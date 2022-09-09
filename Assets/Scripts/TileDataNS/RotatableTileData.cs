using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public abstract class RotatableTileData : TileData
    {
        public Vector2Int Direction { get; set; }
        protected abstract TileSurface UnrotatedSurface { get; }
        public override TileSurface Surface => UnrotatedSurface.RotateTo(Direction);

        protected RotatableTileData(RotatableTileData other) : base(other)
        {
            Direction = other.Direction;
        }

        protected RotatableTileData(Vector2Int position, Vector2Int direction) : base(position)
        {
            Direction = direction;
        }

        public Vector2Int LocalOffsetToGlobalOffset(Vector2Int localOffset)
        {
            var right = Direction;
            var up = Direction.x * Vector2Int.up + Direction.y * Vector2Int.left;
            return localOffset.x * right + localOffset.y * up;
        }

        public TileData GetNeighboringTileByLocalOffset(Vector2Int localOffset)
        {
            var globalOffset = LocalOffsetToGlobalOffset(localOffset);
            return WorkshopData.GetTileAt(Position + globalOffset);
        }
    }
}