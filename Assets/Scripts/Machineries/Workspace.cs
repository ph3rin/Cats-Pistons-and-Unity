using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class Workspace
    {
        public static Vector2Int[] Deltas = new[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1)
        };
        
        private readonly List<Machinery> _machineries;
        private readonly Dictionary<Machinery, List<(Vector2Int, Tile)>> _tilesOfMachineries;
        private readonly Tile[,] _tiles;
        public int Width { get; }
        public int Height { get; }

        public Workspace(IEnumerable<Machinery> machineryApplications, int width, int height)
        {
            _machineries = machineryApplications.ToList();
            _tilesOfMachineries = new Dictionary<Machinery, List<(Vector2Int, Tile)>>();
            _tiles = new Tile[width, height];
            Width = width;
            Height = height;
            UpdateTiles();
        }

        private List<Force> PropagateForces(List<Force> forces, Machinery src)
        {
            Debug.Assert(src == null || _machineries.Contains(src));
            Debug.Assert(forces.All(f => _machineries.Contains(f.Machinery)));
            forces = UniqueForces(forces).ToList();
            var visited = new HashSet<Machinery>(forces.Select(f => f.Machinery));
            var queue = new Queue<Force>(forces);
            while (queue.Count > 0)
            {
                var force = queue.Dequeue();
                var newForces = force
                    .Propagate(this)
                    .Where(f => f.Machinery != src && !visited.Contains(f.Machinery));
                foreach (var newForce in newForces)
                {
                    queue.Enqueue(newForce);
                    forces.Add(newForce);
                    visited.Add(newForce.Machinery);
                }
            }

            return forces;
        }

        public bool ApplyForces(List<Force> forces, Machinery src)
        {
            forces = PropagateForces(forces, src);
            foreach (var force in forces)
            {
                force.Apply(this);
            }

            return UpdateTiles();
        }
        
        public bool UpdateTiles()
        {
            Array.Clear(_tiles, 0, _tiles.Length);
            _tilesOfMachineries.Clear();
            foreach (var machinery in _machineries)
            {
                foreach (var (pos, tile) in machinery.GetTiles())
                {
                    Debug.Assert(tile != null);
                    Debug.Assert(tile.Parent == machinery);
                    if (pos.x < 0 || pos.x >= Width || pos.y < 0 || pos.y >= Height) return false;
                    if (_tiles[pos.x, pos.y] != null) return false;
                    if (!_tilesOfMachineries.ContainsKey(machinery))
                    {
                        _tilesOfMachineries.Add(machinery, new List<(Vector2Int, Tile)>());
                    }
                    _tilesOfMachineries[machinery].Add((pos, tile));
                    _tiles[pos.x, pos.y] = tile;
                }
            }

            return true;
        }

        public IEnumerable<Force> DefaultPropagateForce(Force force)
        {
            var direction = force.Direction;
            Debug.Assert(_tilesOfMachineries.ContainsKey(force.Machinery));
            var machineTiles = _tilesOfMachineries[force.Machinery];
            foreach (var (pos, tile) in machineTiles)
            {
                foreach (var delta in Deltas)
                {
                    var neighborPos = delta + pos;
                    var neighborTile = GetTileAt(neighborPos);
                    if (neighborTile == null) continue;
                    if (machineTiles.FindIndex(p => p.Item2 == neighborTile) != -1) continue;
                    if (delta == direction ||
                        TileSurface.AreGluedTogether(Vector2Int.zero, tile.Surface, delta, neighborTile.Surface))
                    {
                        var neighborMachine = neighborTile.Parent;
                        var neighborLocalPos = neighborPos - neighborMachine.Position;
                        yield return new Force(neighborMachine, direction, neighborLocalPos);
                    }
                }
                
            }            
        }

        public Tile GetTileAt(Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= Width || pos.y < 0 || pos.y >= Height) return null;
            return _tiles[pos.x, pos.y];
        }

        public Tile GetTileAt(int x, int y)
        {
            return GetTileAt(new Vector2Int(x, y));
        }

        private static IEnumerable<Force> UniqueForces(IEnumerable<Force> forces)
        {
            var set = new HashSet<Machinery>();
            foreach (var force in forces)
            {
                if (set.Contains(force.Machinery)) continue;
                set.Add(force.Machinery);
                yield return force;
            }
        }
    }
}