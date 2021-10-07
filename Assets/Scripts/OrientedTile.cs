using UnityEngine;

namespace CatProcessingUnit
{
    public abstract class OrientedTile : WorkshopTile
    {
        [SerializeField] protected Vector2Int _orientation;

        public Vector2Int Orientation
        {
            get => _orientation;
            set => _orientation = value;
        }

        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            transform.localRotation = RotationUtil.OrientationToRotation(_orientation);
        }

        public Vector2Int LocalOffsetToGlobalOffset(Vector2Int localOffset)
        {
            var right = _orientation;
            var up = _orientation.x * Vector2Int.up + _orientation.y * Vector2Int.left;
            return localOffset.x * right + localOffset.y * up;
        }
    }
}