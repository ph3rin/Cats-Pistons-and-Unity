using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public abstract class TileData
    {
        public TileRenderer Renderer { get; set; }
        public Vector2Int Position { get; set; }
        public abstract TileSurface Surface { get; }
        public abstract TileData Clone();

        public virtual void OnActivate()
        {
        }

        public virtual void OnDeactivate()
        {
        }

        protected TileData(TileData other)
        {
            Renderer = other.Renderer;
            Position = other.Position;
        }
    }
}