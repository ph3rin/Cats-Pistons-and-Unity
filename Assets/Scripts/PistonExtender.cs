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

        
        
        public static HashSet<WorkshopTile> GetGluedTiles(
            this Workshop workshop,
            Vector2Int moveDirection,
            HashSet<WorkshopTile> startTiles,
            HashSet<WorkshopTile> staticTiles)
        {
            var visited = new HashSet<WorkshopTile>();
            var queue = new Queue<WorkshopTile>();
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

        public static bool ExtendPiston(WorkshopData data, PistonTile pistonTile,
            Vector2Int direction)
        {
            var startTilePosition = pistonTile.Position + direction;
            var right = pistonTile.GetNeighboringTileByLocalOffset(Vector2Int.right);
            var up = pistonTile.GetNeighboringTileByLocalOffset(Vector2Int.up);
            var down = pistonTile.GetNeighboringTileByLocalOffset(Vector2Int.down);
            var startTiles = new HashSet<WorkshopTile>();
            var staticTiles = new HashSet<WorkshopTile>{pistonTile};
            if (right != null) startTiles.Add(right);
            if (up != null && WorkshopTile.AreGluedTogether(pistonTile, up)) startTiles.Add(up);
            if (down != null && WorkshopTile.AreGluedTogether(pistonTile, down)) startTiles.Add(down);
            var gluedTiles = GetGluedTiles(pistonTile.Workshop, direction, startTiles, staticTiles);
            var tileCoordinates = data.ToCoordinates();
            tileCoordinates.Translate(gluedTiles, direction);

            if (tileCoordinates.IsValidLayout())
            {
                var pistonArm = TileFactory.I.CreatePistonArm(pistonTile.Workshop, pistonTile.Orientation);
                pistonArm.IsSticky = pistonTile.IsSticky;
                tileCoordinates.SetTilePosition(pistonArm, startTilePosition);
                data.ApplyCoordinates(tileCoordinates);
                return true;
            }

            return false;
        }

        public static bool RetractPiston(WorkshopData data, PistonTile pistonTile, Vector2Int pistonDirection)
        {
            var armTilePosition = pistonTile.Position + pistonDirection;
            var armTile = data.GetTileAt(armTilePosition) as PistonArmTile;
            Debug.Assert(armTile != null);
            var right =  armTile.GetNeighboringTileByLocalOffset(Vector2Int.right);
            var up = armTile.GetNeighboringTileByLocalOffset(Vector2Int.up);
            var down = armTile.GetNeighboringTileByLocalOffset(Vector2Int.down);
            var startTiles = new HashSet<WorkshopTile>();
            var staticTiles = new HashSet<WorkshopTile> {pistonTile, armTile};
            if (right != null && WorkshopTile.AreGluedTogether(armTile, right)) startTiles.Add(right);
            if (up != null && WorkshopTile.AreGluedTogether(armTile, up)) startTiles.Add(up);
            if (down != null && WorkshopTile.AreGluedTogether(armTile, down)) startTiles.Add(down);
            var gluedTiles = GetGluedTiles(pistonTile.Workshop, -pistonDirection, startTiles, staticTiles);
            
            var tileCoordinates = data.ToCoordinates();
            tileCoordinates.SetTilePosition(armTile, null);
            gluedTiles.Remove(armTile);
            tileCoordinates.Translate(gluedTiles, -pistonDirection);

            if (tileCoordinates.IsValidLayout())
            {
                data.ApplyCoordinates(tileCoordinates);
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