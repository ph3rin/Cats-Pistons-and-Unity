using UnityEngine;
using UnityEngine.Serialization;

namespace CatProcessingUnit
{
    public abstract class OrientedTile : WorkshopTile
    {
        [FormerlySerializedAs("_orientation")] [SerializeField] protected Vector2Int _direction;

        public Vector2Int Orientation
        {
            get => _direction;
            set => _direction = value;
        }
        
        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            transform.localRotation = RotationUtil.OrientationToRotation(_direction);
        }

        public Vector2Int LocalOffsetToGlobalOffset(Vector2Int localOffset)
        {
            var right = _direction;
            var up = _direction.x * Vector2Int.up + _direction.y * Vector2Int.left;
            return localOffset.x * right + localOffset.y * up;
        }

        public WorkshopTile GetNeighboringTileByLocalOffset(Vector2Int localOffset)
        {
            var globalOffset = LocalOffsetToGlobalOffset(localOffset);
            return Workshop.GetTileAt(Position + globalOffset);
        }
    }
}