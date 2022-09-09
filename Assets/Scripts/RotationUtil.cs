using System;
using System.Collections.Generic;
using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit
{
    public static class RotationUtil
    {
        public static Quaternion OrientationToRotation(Vector2Int orientation)
        {
            if (orientation == Vector2Int.right)
            {
                return Quaternion.identity;
            }
            else if (orientation == Vector2Int.up)
            {
                return Quaternion.Euler(0, 0, 90.0f);
            }
            else if (orientation == Vector2Int.left)
            {
                return Quaternion.Euler(0, 0, 180.0f);
            }
            else if (orientation == Vector2Int.down)
            {
                return Quaternion.Euler(0, 0, -90.0f);
            }

            throw new ArgumentOutOfRangeException(nameof(orientation));
        }

        public static Vector2Int RightVectorToDirection(Vector2 right)
        {
            return new Vector2Int(Mathf.RoundToInt(right.x), Mathf.RoundToInt(right.y));
        }

        public static void AddNeighborIfGluedTo(this RotatableTileData self, Vector2Int offset, HashSet<TileData> set)
        {
            var neighbor = self.GetNeighboringTileByLocalOffset(offset);
            if (neighbor != null && TileData.AreGluedTogether(neighbor, self))
            {
                set.Add(neighbor);
            }
        }
    }
}