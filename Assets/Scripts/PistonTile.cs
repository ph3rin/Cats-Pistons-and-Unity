using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit
{
    public class PistonTile : OrientedTile, IPointerClickHandler
    {
        [SerializeField] private Sprite _retractedSprite;
        [SerializeField] private Sprite _retractedStickySprite;
        [SerializeField] private Sprite _extendedSprite;
        [SerializeField] private Sprite _extendedStickySprite;

        [SerializeField] private bool _extended;

        public bool IsSticky { get; set; }

        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            _spriteRenderer.sprite = _extended
                ? (IsSticky ? _extendedStickySprite : _extendedSprite)
                : (IsSticky ? _retractedStickySprite : _retractedSprite);
        }

        private void Extend()
        {
            Debug.Assert(!_extended);
            if (PistonExtender.ExtendPiston(Workshop.TileData, this, _orientation))
            {
                _extended = true;
                Workshop.Refresh();
                FindObjectOfType<AudioManager>().Play("extendPiston");
            }
        }

        private void Retract()
        {
            Debug.Assert(_extended);
            if (PistonExtender.RetractPiston(Workshop.TileData, this, _orientation))
            {
                _extended = false;
                Workshop.Refresh();
                FindObjectOfType<AudioManager>().Play("retractPiston");
            }
        }

        public override bool IsStickyOnOrientation(Vector2Int orientation)
        {
            return IsSticky && orientation == _orientation;
        }

        public void ToggleStickiness()
        {
            IsSticky = !IsSticky;
            if (_extended)
            {
                ((PistonArmTile) Workshop.GetTileAt(Position + _orientation)).IsSticky = IsSticky;
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