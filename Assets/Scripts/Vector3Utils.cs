using UnityEngine;

namespace CatProcessingUnit
{
    public static class Vector3Utils
    {
        public static Vector3 CameraPos(this Transform transform)
        {
            var pos = transform.position;
            pos.z = -10;
            return pos;
        }
    }
}