using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace CatProcessingUnit
{
    public class PistonTile : OrientedTile, IPointerClickHandler
    {
        [SerializeField] private bool _extended;
        [SerializeField] private SpriteConfig _sprites;
        
        public bool IsSticky { get; set; }

        public override TileSurface Surface => UnrotatedSurface.RotateTo(_direction);

        private TileSurface UnrotatedSurface
        {
            get
            {
                if (_extended) return TileSurface.PistonExtended;
                if (IsSticky) return TileSurface.PistonSticky;
                return TileSurface.Solid;
            }
        }

        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            _spriteRenderer.sprite = _sprites.GetPistonSprite(IsSticky, _extended);
        }

        private void Extend()
        {
            Debug.Assert(!_extended);
            if (PistonExtender.ExtendPiston(Workshop.Data, this, _direction))
            {
                _extended = true;
                Workshop.Refresh();
                FindObjectOfType<AudioManager>().Play("extendPiston");
            }
        }

        private void Retract()
        {
            Debug.Assert(_extended);
            if (PistonExtender.RetractPiston(Workshop.Data, this, _direction))
            {
                _extended = false;
                Workshop.Refresh();
                FindObjectOfType<AudioManager>().Play("retractPiston");
            }
        }

        public override bool IsStickyOnOrientation(Vector2Int orientation)
        {
            return IsSticky && orientation == _direction;
        }

        public void ToggleStickiness()
        {
            IsSticky = !IsSticky;
            if (_extended)
            {
                ((PistonArmTile) Workshop.GetTileAt(Position + _direction)).IsSticky = IsSticky;
            }

            AudioManager.I.Play("stickySwitch");
            Workshop.Refresh();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                ToggleExtension();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                ToggleStickiness();
            }
        }

        public void ToggleExtension()
        {
            if (_extended)
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