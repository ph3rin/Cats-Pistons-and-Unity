using System.Collections.Generic;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public class PistonTileData : RotatableTileData
    {
        public bool Extended { get; private set; } = false;
        public bool Sticky { get; set; } = false;

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
            var animations = new List<DeferredAnimationInstruction>();
            if (PistonExtender.ExtendPiston(newData, newPiston, Direction, animations))
            {
                newPiston.Extended = true;
                newData.PushToWorkshopHistory();
                var animationManager = ServiceLocator.GetService<AnimationManager>();
                foreach (var animation in animations)
                {
                    animation.Tile.Renderer.QueueAnimation(animation.Instruction);
                    animationManager.AddAnimationFinishedCallback(animation.Tile.OnPostAnimation);
                }

                animationManager.AddAnimationFinishedCallback(() => WorkshopData.Workshop.RemoveUnusedRenderer());
                animationManager.PlayAll();
            }
        }

        public void Retract()
        {
            Debug.Assert(Extended);
            var newData = WorkshopData.Clone();
            var newPiston = newData.GetTileAt(Position) as PistonTileData;
            Debug.Assert(newPiston != null);
            var animations = new List<DeferredAnimationInstruction>();
            if (PistonExtender.RetractPiston(newData, newPiston, Direction, animations))
            {
                newPiston.Extended = false;
                newData.PushToWorkshopHistory();
                var animationManager = ServiceLocator.GetService<AnimationManager>();
                foreach (var animation in animations)
                {
                    animation.Tile.Renderer.QueueAnimation(animation.Instruction);
                    animationManager.AddAnimationFinishedCallback(animation.Tile.OnPostAnimation);
                }

                animationManager.AddAnimationFinishedCallback(() => WorkshopData.Workshop.RemoveUnusedRenderer());
                animationManager.PlayAll();
            }
        }

        public override void OnActivate()
        {
            base.OnActivate();
            Renderer.onLeftClick += ToggleExtension;
            Renderer.onRightClick += ToggleStickiness;
            Debug.Assert(_renderer != null, "_renderer != null");
        }

        public override void OnPostAnimation()
        {
            base.OnPostAnimation();
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