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
            Sticky ? TileSurface.PistonArmSticky : TileSurface.PistonArm;

        public PistonArmTileData(PistonArmTileData other) : base(other)
        {
            Sticky = other.Sticky;
            _renderer = other._renderer;
        }

        public override TileData Clone()
        {
            return new PistonArmTileData(this);
        }

        public PistonArmTileData(Vector2Int position, Vector2Int direction, bool sticky) :
            base(position, direction)
        {
            Sticky = sticky;
        }

        public override void OnActivate()
        {
            base.OnActivate();
            if (_renderer == null)
            {
                _renderer = TileFactory.I.CreatePistonArmRenderer(this);
            }
            _renderer.gameObject.SetActive(true);
        }

        public override void OnPostAnimation()
        {
            base.OnPostAnimation();
            _renderer.Render(this);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }
    }
}