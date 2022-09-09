using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public class PistonArmTileData : RotatableTileData
    {
        public bool Sticky { get; set; }

        private PistonArmTileRenderer _renderer = null;
        public override TileRenderer Renderer => _renderer;

        protected override TileSurface UnrotatedSurface =>
            Sticky ? TileSurface.PistonHeadSticky : TileSurface.PistonHead;

        public bool IsStem { get; set; }

        public PistonArmTileData(PistonArmTileData other) : base(other)
        {
            Sticky = other.Sticky;
            _renderer = other._renderer;
            IsStem = other.IsStem;
        }

        public override TileData Clone()
        {
            return new PistonArmTileData(this);
        }

        public PistonArmTileData(Vector2Int position, Vector2Int direction, bool sticky, bool isStem = false) :
            base(position, direction)
        {
            Sticky = sticky;
            IsStem = isStem;
        }

        public override void OnActivate()
        {
            base.OnActivate();
            if (_renderer == null)
            {
                _renderer = TileFactory.I.CreatePistonArmRenderer(this);
            }
            _renderer.gameObject.SetActive(true);
            _renderer.Render(this);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            _renderer.gameObject.SetActive(false);
        }
    }
}