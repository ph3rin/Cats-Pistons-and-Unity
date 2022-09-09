using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public class PistonTileData : RotatableTileData
    {
        public bool Extended => CurrentLength > 0;
        public bool Sticky { get; set; } = false;

        public int MaxLength { get; }

        public int CurrentLength { get; private set; } = 0;

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
            Sticky = other.Sticky;
            MaxLength = other.MaxLength;
            CurrentLength = other.CurrentLength;
            _renderer = other._renderer;
        }

        public PistonTileData(Vector2Int position, Vector2Int direction, int maxLength = 1) : base(position, direction)
        {
            MaxLength = maxLength;
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
            var lengthChanged = false;
            for (var i = newPiston.CurrentLength; i < newPiston.MaxLength; ++i)
            {
                if (!PistonExtender.ExtendPiston(newData, newPiston, Direction)) break;
                ++newPiston.CurrentLength;
                lengthChanged = true;
            }

            if (lengthChanged)
            {
                newData.PushToWorkshopHistory();
            }
        }

        public void Retract()
        {
            Debug.Assert(Extended);
            var newData = WorkshopData.Clone();
            var newPiston = newData.GetTileAt(Position) as PistonTileData;
            Debug.Assert(newPiston != null);
            var lengthChanged = false;
            for (var i = newPiston.CurrentLength; i > 0; --i)
            {
                if (!PistonExtender.RetractPiston(newData, newPiston, Direction)) break;
                --newPiston.CurrentLength;
                if (newPiston.FindArmTile() is PistonArmTileData realArm)
                {
                    realArm.IsStem = false;
                }
                lengthChanged = true;
            }

            if (lengthChanged)
            {
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
            newData.TogglePistonStickiness(this);
            // var newPistonTile = newData.GetTileAt(Position) as PistonTileData;
            // Debug.Assert(newPistonTile != null);
            // newPistonTile.Sticky = !newPistonTile.Sticky;
            // if (Extended)
            // {
            //     var newArmTile = newData.GetTileAt(Position + Direction) as PistonArmTileData;
            //     newArmTile.Sticky = !newArmTile.Sticky;
            // }
            //
            // if (WorkshopData.StickyPiston != null && WorkshopData.StickyPiston != this)
            // {
            //     PistonTileData otherPiston = WorkshopData.StickyPiston;
            //     newData.StickyPiston = newPistonTile;
            //     var otherTile = newData.GetTileAt(otherPiston.Position) as PistonTileData;
            //     otherTile.Sticky = !otherTile.Sticky;
            //     if (otherPiston.Extended)
            //     {
            //         var otherArmTile = newData.GetTileAt(otherPiston.Position + otherPiston.Direction) as PistonArmTileData;
            //         otherArmTile.Sticky = !otherArmTile.Sticky;
            //     }
            // }
            // else if (WorkshopData.StickyPiston == null)
            // {
            //     newData.StickyPiston = newPistonTile;
            // }
            // else if (WorkshopData.StickyPiston == this)
            // {
            //     newData.StickyPiston = null;
            // }

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