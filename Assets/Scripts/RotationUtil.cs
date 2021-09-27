using System;
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
    }
}