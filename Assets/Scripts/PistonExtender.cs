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
                if (!staticTiles.Contains(startTile))
                {
                    visited.Add(startTile);
                }

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

        public static WorkshopTileCoordinates MovePistonArm(
            WorkshopData data,
            RotatableTileData armTile,
            int directionSign)
        {
            Debug.Assert(directionSign == -1 || directionSign == 1);
            armTile = data.FindCounterpart(armTile);
            var direction = armTile.Direction * directionSign;
            var startTiles = new HashSet<TileData> {armTile};
            var staticTiles = FindAllTilesInPiston(armTile);
            var gluedTiles = data.GetGluedTiles(direction, startTiles, staticTiles);
            var coordinates = data.ToCoordinates();
            coordinates.Translate(gluedTiles, direction);
            return coordinates;
        }

        private static HashSet<TileData> FindAllTilesInPiston(RotatableTileData armTile)
        {
            var result = new HashSet<TileData>();
            while (true)
            {
                result.Add(armTile);
                if (armTile is PistonTileData) return result;
                armTile = armTile.GetNeighboringTileByLocalOffset(Vector2Int.left) as RotatableTileData;
                Debug.Assert(armTile != null);
            }
        }

        public static bool ExtendPiston(WorkshopData data, PistonTileData pistonTile,
            Vector2Int direction)
        {
            pistonTile = data.FindCounterpart(pistonTile);
            var armTile = FindArmTile(pistonTile);
            var armSpawnPos = armTile.Position + armTile.Direction;
            var tileCoordinates = MovePistonArm(data, armTile, 1);
            Debug.Assert(data.Tiles.Contains(pistonTile));
            if (tileCoordinates.IsValidLayout())
            {
                var pistonArm = new PistonArmTileData(armSpawnPos, pistonTile.Direction, pistonTile.Sticky)
                {
                    WorkshopData = data
                };
                tileCoordinates.SetTilePosition(pistonArm, armSpawnPos);
                if (armTile is PistonArmTileData realArm)
                {
                    realArm.IsStem = true;
                }
                data.ApplyCoordinates(tileCoordinates);
                return true;
            }

            return false;
        }

        public static bool RetractPiston(WorkshopData data, PistonTileData pistonTile, Vector2Int pistonDirection)
        {
            Debug.Assert(data.Tiles.Contains(pistonTile));
            var armTile = FindArmTile(pistonTile);
            Debug.Assert(armTile != null);

            var tileCoordinates = MovePistonArm(data, armTile, -1);
            tileCoordinates.SetTilePosition(armTile, null);
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

        public static RotatableTileData FindArmTile(this PistonTileData pistonTile)
        {
            var result =
                pistonTile.GetNeighboringTileByLocalOffset(Vector2Int.right * pistonTile.CurrentLength) as
                    RotatableTileData;
            Debug.Assert(result != null);
            return result;
        }
    }
}