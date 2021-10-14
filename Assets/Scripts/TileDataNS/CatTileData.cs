using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public class CatTileData : TileData
    {
        public override TileRenderer Renderer => _renderer;

        private TileRenderer _renderer;

        public CatTileData SetRenderer(TileRenderer renderer)
        {
            _renderer = renderer;
            return this;
        }
         
        public override TileSurface Surface => TileSurface.Solid;
        
        protected CatTileData(CatTileData other) : base(other)
        {
            _renderer = other._renderer;
        }
        
        public override TileData Clone()
        {
            return new CatTileData(this);
        }

        public override void OnActivate()
        {
            base.OnActivate();
            Debug.Assert(_renderer != null);
            _renderer.Render(this);
        }

        public CatTileData(Vector2Int position) : base(position)
        {
        }
    }
}