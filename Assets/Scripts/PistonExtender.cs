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

        private static HashSet<WorkshopTile> GetGluedBlocks(WorkshopTile startTile, Vector2Int moveDirection,
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
                var position = front.Position;
                foreach (var delta in Deltas)
                {
                    var neighborPosition = position + delta;
                    var neighborTile = workshop.GetTileAt(neighborPosition);
                    if (neighborTile == null || visited.Contains(neighborTile) || neighborTile == ignoreTileA ||
                        neighborTile == ignoreTileB) continue;
                    if (delta != moveDirection && !front.IsStickyOnOrientation(delta) &&
                        !neighborTile.IsStickyOnOrientation(-delta)) continue;
                    visited.Add(neighborTile);
                    queue.Enqueue(neighborTile);
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
                var gluedTiles = GetGluedBlocks(startTile, direction, pistonTile);
                tileCoordinates.Translate(gluedTiles, direction);
            }

            var pistonArm = TileFactory.I.CreatePistonArm(pistonTile.Workshop, pistonTile.Orientation);
            pistonArm.IsSticky = pistonTile.IsSticky;
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
                var gluedTiles = GetGluedBlocks(armTile, -orientation, pistonTile, armTile);
                gluedTiles.Remove(armTile);
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