using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public abstract class RotatableTileData : TileData
    {
        public Vector2Int Direction { get; set; }
        protected abstract TileSurface UnrotatedSurface { get; }
        public override TileSurface Surface => UnrotatedSurface.RotateTo(Direction);
    }
}