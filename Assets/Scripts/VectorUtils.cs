using UnityEngine;

namespace CatProcessingUnit
{
    public static class VectorUtils
    {
        public static Vector2Int RountToInt(this Vector3 vector3)
        {
            return new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
        }
    }
}