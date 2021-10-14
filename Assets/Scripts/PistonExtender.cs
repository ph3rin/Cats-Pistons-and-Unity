using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit
{
    public static class PistonExtender
    {
        private static Vector2Int[] Deltas = new[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1)
        };


        public static HashSet<TileData> GetGluedTiles(
            this WorkshopData workshop,
            Vector2Int moveDirection,
            HashSet<TileData> startTiles,
            HashSet<TileData> staticTiles)
        {
            var visited = new HashSet<TileData>();
            var queue = new Queue<TileData>();
            foreach (var startTile in startTiles)
            {
                visited.Add(startTile);
                queue.Enqueue(startTile);
            }
            while (queue.Count > 0)
            {
                var front = queue.Dequeue();
                var position = front.Position;
                foreach (var delta in Deltas)
                {
                    var neighborPosition = position + delta;
                    var neighbor = workshop.GetTileAt(neighborPosition);
                    if (neighbor == null || visited.Contains(neighbor) || staticTiles.Contains(neighbor)) continue;
                    if (delta == moveDirection ||
                        TileSurface.AreGluedTogether(Vector2Int.zero, front.Surface, delta, neighbor.Surface))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return visited;
        }

        public static bool ExtendPiston(WorkshopData data, PistonTileData pistonTile,
            Vector2Int direction)
        {
            Debug.Assert(data.Tiles.Contains(pistonTile));
            var startTilePosition = pistonTile.Position + direction;
            var right = pistonTile.GetNeighboringTileByLocalOffset(Vector2Int.right);
            var up = pistonTile.GetNeighboringTileByLocalOffset(Vector2Int.up);
            var down = pistonTile.GetNeighboringTileByLocalOffset(Vector2Int.down);
            var startTiles = new HashSet<TileData>();
            var staticTiles = new HashSet<TileData>{pistonTile};
            if (right != null) startTiles.Add(right);
            if (up != null && TileData.AreGluedTogether(pistonTile, up)) startTiles.Add(up);
            if (down != null && TileData.AreGluedTogether(pistonTile, down)) startTiles.Add(down);
            var gluedTiles = GetGluedTiles(data, direction, startTiles, staticTiles);
            var tileCoordinates = data.ToCoordinates();
            tileCoordinates.Translate(gluedTiles, direction);

            if (tileCoordinates.IsValidLayout())
            {
                var pistonArm = new PistonArmTileData(startTilePosition, pistonTile.Direction, pistonTile.Sticky)
                {
                    WorkshopData = data
                };
                tileCoordinates.SetTilePosition(pistonArm, startTilePosition);
                data.ApplyCoordinates(tileCoordinates);
                return true;
            }

            return false;
        }

        public static bool RetractPiston(WorkshopData data, PistonTileData pistonTile, Vector2Int pistonDirection)
        {
            Debug.Assert(data.Tiles.Contains(pistonTile));
            var armTilePosition = pistonTile.Position + pistonDirection;
            var armTile = data.GetTileAt(armTilePosition) as PistonArmTileData;
            Debug.Assert(armTile != null);
            var right =  armTile.GetNeighboringTileByLocalOffset(Vector2Int.right);
            var up = armTile.GetNeighboringTileByLocalOffset(Vector2Int.up);
            var down = armTile.GetNeighboringTileByLocalOffset(Vector2Int.down);
            var startTiles = new HashSet<TileData>();
            var staticTiles = new HashSet<TileData> {pistonTile, armTile};
            if (right != null && TileData.AreGluedTogether(armTile, right)) startTiles.Add(right);
            if (up != null && TileData.AreGluedTogether(armTile, up)) startTiles.Add(up);
            if (down != null && TileData.AreGluedTogether(armTile, down)) startTiles.Add(down);
            var gluedTiles = GetGluedTiles(data, -pistonDirection, startTiles, staticTiles);
            
            var tileCoordinates = data.ToCoordinates();
            tileCoordinates.SetTilePosition(armTile, null);
            gluedTiles.Remove(armTile);
            tileCoordinates.Translate(gluedTiles, -pistonDirection);

            if (tileCoordinates.IsValidLayout())
            {
                data.ApplyCoordinates(tileCoordinates);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}