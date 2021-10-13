namespace CatProcessingUnit.TileDataNS
{
    public class PistonTileData : RotatableTileData
    {
        public bool Extended { get; private set; }
        public bool Sticky { get; private set; }
        
        protected override TileSurface UnrotatedSurface
        {
            get
            {
                if (Extended) return TileSurface.PistonExtended;
                if (Sticky) return TileSurface.PistonSticky;
                return TileSurface.Solid;
            }
        }

        public PistonTileData(PistonTileData other) : base(other)
        {
            Extended = other.Extended;
            Sticky = other.Sticky;
        }

        public override TileData Clone()
        {
            return new PistonTileData(this);
        }
    }
}