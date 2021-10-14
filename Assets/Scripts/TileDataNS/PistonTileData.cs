using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public class PistonTileData : RotatableTileData
    {
        public bool Extended { get; private set; } = false;
        public bool Sticky { get; private set; } = false;

        private PistonTileRenderer _renderer;

        public void SetRenderer(PistonTileRenderer renderer)
        {
            _renderer = renderer;
        }

        public override TileRenderer Renderer => _renderer;

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
            _renderer = other._renderer;
        }

        public PistonTileData(Vector2Int position, Vector2Int direction) : base(position, direction)
        {
        }

        public override TileData Clone()
        {
            return new PistonTileData(this);
        }

        public void Extend()
        {
            Debug.Assert(!Extended);
            var newData = WorkshopData.Clone();
            var newPiston = newData.GetTileAt(Position) as PistonTileData;
            Debug.Assert(newPiston != null);
            if (PistonExtender.ExtendPiston(newData, newPiston, Direction))
            {
                newPiston.Extended = true;
                newData.PushToWorkshopHistory();
            }
        }

        public void Retract()
        {
            Debug.Assert(Extended);
            var newData = WorkshopData.Clone();
            var newPiston = newData.GetTileAt(Position) as PistonTileData;
            Debug.Assert(newPiston != null);
            if (PistonExtender.RetractPiston(newData, newPiston, Direction))
            {
                newPiston.Extended = false;
                newData.PushToWorkshopHistory();
            }
        }

        public override void OnActivate()
        {
            base.OnActivate();
            Renderer.onLeftClick += ToggleExtension;
            Renderer.onRightClick += ToggleStickiness;
            Debug.Assert(_renderer != null, "_renderer != null");
            _renderer.Render(this);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            Renderer.onLeftClick -= ToggleExtension;
            Renderer.onRightClick -= ToggleStickiness;
        }
        

        private void ToggleStickiness()
        {
            var newData = WorkshopData.Clone();
            var newPistonTile = newData.GetTileAt(Position) as PistonTileData;
            Debug.Assert(newPistonTile != null);
            newPistonTile.Sticky = !newPistonTile.Sticky;
            if (Extended)
            {
                var newArmTile = newData.GetTileAt(Position + Direction) as PistonArmTileData;
                newArmTile.Sticky = !newArmTile.Sticky;
            }
            newData.PushToWorkshopHistory();
        }

        private void ToggleExtension()
        {
            if (Extended)
            {
                Retract();
            }
            else
            {
                Extend();
            }
        }
    }
}