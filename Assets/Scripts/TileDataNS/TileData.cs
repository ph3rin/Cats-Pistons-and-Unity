using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public abstract class TileData
    {
        public WorkshopData WorkshopData { get; set; } = null;
        public abstract TileRenderer Renderer { get; }
        public Vector2Int Position { get; set; }
        public abstract TileSurface Surface { get; }
        public abstract TileData Clone();

        public virtual void OnActivate()
        {
        }

        public virtual void OnDeactivate()
        {
        }

        public virtual void OnPostAnimation()
        {
        }

        protected TileData(TileData other)
        {
            Position = other.Position;
        }

        protected TileData(Vector2Int position)
        {
            Position = position;
        }

        public static bool AreGluedTogether(TileData tileA, TileData tileB)
        {
            Debug.Assert(tileA != null);
            Debug.Assert(tileB != null);

            return TileSurface.AreGluedTogether(tileA.Position, tileA.Surface, tileB.Position, tileB.Surface);
        }
    }
}