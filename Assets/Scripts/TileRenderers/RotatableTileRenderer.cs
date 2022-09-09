using CatProcessingUnit.TileDataNS;

namespace CatProcessingUnit.TileRenderers
{
    public abstract class RotatableTileRenderer : TileRenderer
    {
        protected void Render(RotatableTileData tileData)
        {
            base.Render(tileData);
            transform.localRotation = RotationUtil.OrientationToRotation(tileData.Direction);
        }
    }
}