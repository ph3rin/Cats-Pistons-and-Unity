using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.AnimationInstructions;
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
            int directionSign,
            List<DeferredAnimationInstruction> generatedAnimations)
        {
            generatedAnimations.Clear();
            Debug.Assert(directionSign == -1 || directionSign == 1);
            armTile = data.FindCounterpart(armTile);
            var direction = armTile.Direction * directionSign;
            var startTiles = new HashSet<TileData> {armTile};
            var staticTiles = FindAllTilesInPiston(armTile);
            var gluedTiles = data.GetGluedTiles(direction, startTiles, staticTiles);
            var coordinates = data.ToCoordinates();
            coordinates.Translate(gluedTiles, direction);
            var translateInstruction = new AnimationTranslate(direction);
            generatedAnimations.AddRange(
                gluedTiles.Select(gluedTile => new DeferredAnimationInstruction(translateInstruction, gluedTile)));
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

        public static bool ExtendPiston(
            WorkshopData data,
            PistonTileData pistonTile,
            Vector2Int direction,
            List<DeferredAnimationInstruction> generatedAnimations)
        {
            var tileCoordinates = MovePistonArm(data, pistonTile, 1, generatedAnimations);

            Debug.Assert(data.Tiles.Contains(pistonTile));
            var startTilePosition = pistonTile.Position + direction;
            if (tileCoordinates.IsValidLayout())
            {
                var pistonArm = new PistonArmTileData(startTilePosition, pistonTile.Direction, pistonTile.Sticky)
                {
                    WorkshopData = data
                };
                tileCoordinates.SetTilePosition(pistonArm, startTilePosition);
                generatedAnimations.Add(new DeferredAnimationInstruction(
                    new AnimationPush(), pistonArm));
                data.ApplyCoordinates(tileCoordinates);
                return true;
            }

            return false;
        }

        public static bool RetractPiston(
            WorkshopData data,
            PistonTileData pistonTile,
            Vector2Int pistonDirection,
            List<DeferredAnimationInstruction> generatedAnimations)
        {
            Debug.Assert(data.Tiles.Contains(pistonTile));
            var armTilePosition = pistonTile.Position + pistonDirection;
            var armTile = data.GetTileAt(armTilePosition) as PistonArmTileData;
            Debug.Assert(armTile != null);

            var tileCoordinates = MovePistonArm(data, armTile, -1, generatedAnimations);
            tileCoordinates.SetTilePosition(armTile, null);
            generatedAnimations.Add(new DeferredAnimationInstruction(
                new AnimationPull(), armTile));
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