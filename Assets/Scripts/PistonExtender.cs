using System.Collections.Generic;
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

        private static HashSet<WorkshopTile> GetGluedBlocks(WorkshopTile startTile, 
            WorkshopTile ignoreTileA, WorkshopTile ignoreTileB = null)
        {
            Debug.Assert(startTile != null);
            var workshop = startTile.Workshop;
            var visited = new HashSet<WorkshopTile> {startTile};
            var queue = new Queue<WorkshopTile>();
            queue.Enqueue(startTile);
            while (queue.Count > 0)
            {
                var front = queue.Dequeue();
                foreach (var delta in Deltas)
                {
                    var position = front.Position;
                    position += delta;
                    var tile = workshop.GetTileAt(position);
                    if (tile == null || visited.Contains(tile) || tile == ignoreTileA || tile == ignoreTileB) continue;
                    visited.Add(tile);
                    queue.Enqueue(tile);
                }
            }

            return visited;
        }

        public static bool ExtendPiston(WorkshopTileData tileData, PistonTile pistonTile,
            Vector2Int direction)
        {
            var startTilePosition = pistonTile.Position + direction;
            var startTile = tileData.GetTileAt(startTilePosition);
            var tileCoordinates = tileData.ToCoordinates();
            if (startTile != null)
            {
                var gluedTiles = GetGluedBlocks(startTile, pistonTile);
                tileCoordinates.Translate(gluedTiles, direction);
            }

            var pistonArm = TileFactory.I.CreatePistonArm(pistonTile.Workshop, pistonTile.Orientation);
            tileCoordinates.SetTilePosition(pistonArm, startTilePosition);
            if (tileCoordinates.IsValidLayout())
            {
                tileData.ApplyCoordinates(tileCoordinates);
                return true;
            }
            else
            {
                Object.Destroy(pistonArm.gameObject);
                return false;
            }
        }

        public static bool RetractPiston(WorkshopTileData tileData, PistonTile pistonTile, Vector2Int orientation)
        {
            var armTilePosition = pistonTile.Position + orientation;
            var pullTilePosition = armTilePosition + orientation;
            var armTile = tileData.GetTileAt(armTilePosition);
            var pullTile = tileData.GetTileAt(pullTilePosition);
            var tileCoordinates = tileData.ToCoordinates();
            tileCoordinates.SetTilePosition(armTile, null);
            if (pullTile != null)
            {
                var gluedTiles = GetGluedBlocks(pullTile, pistonTile, armTile);
                tileCoordinates.Translate(gluedTiles, -orientation);
            }
            if (tileCoordinates.IsValidLayout())
            {
                tileData.ApplyCoordinates(tileCoordinates);
                Object.Destroy(armTile.gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}