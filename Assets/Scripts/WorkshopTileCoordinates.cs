using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit
{
    public class WorkshopTileCoordinates
    {
        private readonly int _width;
        private readonly int _height;

        public IDictionary<TileData, Vector2Int> Coordinates => _tileCoordinates;

        public WorkshopTileCoordinates(TileData[,] tiles)
        {
            _width = tiles.GetLength(0);
            _height = tiles.GetLength(1);
            _tileCoordinates = new Dictionary<TileData, Vector2Int>();
            for (var x = 0; x < _width; ++x)
            {
                for (var y = 0; y < _height; ++y)
                {
                    var tile = tiles[x, y];
                    if (tile == null) continue;
                    _tileCoordinates.Add(tile, new Vector2Int(x, y));
                }
            }
        }

        private Dictionary<TileData, Vector2Int> _tileCoordinates;

        public Vector2Int? GetTilePosition(TileData tile)
        {
            Debug.Assert(tile != null);
            if (_tileCoordinates.TryGetValue(tile, out var position))
            {
                return position;
            }

            return null;
        }

        public void SetTilePosition(TileData tile, Vector2Int? pos)
        {
            if (pos is { } newPos)
            {
                _tileCoordinates[tile] = newPos;
            }
            else
            {
                _tileCoordinates.Remove(tile);
            }
        }

        public void Translate(HashSet<TileData> gluedTiles, Vector2Int direction)
        {
            foreach (var tile in gluedTiles)
            {
                Debug.Assert(tile != null);
                SetTilePosition(tile, GetTilePosition(tile) + direction);
            }
        }

        public bool IsValidLayout()
        {
            var occupied = new bool[_width, _height];
            foreach (var pair in _tileCoordinates)
            {
                var tile = pair.Key;
                var pos = pair.Value;
                if (IsOutOfBounds(pos)) return false;
                if (occupied[pos.x, pos.y]) return false;
                occupied[pos.x, pos.y] = true;
            }

            return true;
        }

        private bool IsOutOfBounds(Vector2Int pos)
        {
            return pos.x < 0 || pos.x >= _width || pos.y < 0 || pos.y >= _height;
        }
    }
}