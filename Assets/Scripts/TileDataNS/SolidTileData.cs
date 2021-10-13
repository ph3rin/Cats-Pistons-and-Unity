namespace CatProcessingUnit.TileDataNS
{
    public class SolidTileData : TileData
    {
        public override TileSurface Surface => TileSurface.SolidSticky;

        public SolidTileData(SolidTileData other) : base(other)
        {
        }

        public override TileData Clone()
        {
            return new SolidTileData(this);
        }
    }
}