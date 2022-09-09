using System;
using UnityEngine;

namespace CatProcessingUnit
{
    public static class DirectionUtils
    {
        private static readonly int[,] _dirIndexMap;

        static DirectionUtils()
        {
            _dirIndexMap = new int[3, 3];
            for (var x = 0; x < 3; ++x)
            {
                for (var y = 0; y < 3; ++y)
                {
                    _dirIndexMap[x, y] = -1;
                }
            } 
            _dirIndexMap[2, 1] = 0;
            _dirIndexMap[1, 2] = 1;
            _dirIndexMap[0, 1] = 2;
            _dirIndexMap[1, 0] = 3;
        }

        public static int ManhattanLength(this Vector2Int vec)
        {
            return Mathf.Abs(vec.x) + Mathf.Abs(vec.y);
        }

        public static int DirectionToIndex(Vector2Int dir)
        {
            Debug.Assert(ManhattanLength(dir) == 1);
            dir += Vector2Int.one;
            var idx = _dirIndexMap[dir.x, dir.y];
            Debug.Assert(idx != -1);
            return idx;
        } 
    }
}