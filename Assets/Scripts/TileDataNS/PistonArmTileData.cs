namespace CatProcessingUnit.TileDataNS
{
    public class PistonArmTileData : RotatableTileData
    {
        public bool Sticky { get; private set; }

        protected override TileSurface UnrotatedSurface =>
            Sticky ? TileSurface.PistonArmSticky : TileSurface.PistonArm;
    }
}