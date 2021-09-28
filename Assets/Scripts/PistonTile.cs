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

        [SerializeField] private bool _extended;

        public bool IsSticky { get; set; }

        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            _spriteRenderer.sprite = _extended ? _extendedSprite :
                IsSticky ? _retractedStickySprite : _retractedSprite;
        }

        private void Extend()
        {
            Debug.Assert(!_extended);
            if (PistonExtender.ExtendPiston(Workshop.TileData, this, _orientation))
            {
                _extended = true;
                Workshop.RefreshTileRenderers();
            }
        }

        private void Retract()
        {
            Debug.Assert(_extended);
            if (PistonExtender.RetractPiston(Workshop.TileData, this, _orientation))
            {
                _extended = false;
                Workshop.RefreshTileRenderers();
            }
        }

        public override bool IsStickyOnOrientation(Vector2Int orientation)
        {
            return IsSticky && orientation == _orientation;
        }

        // private void OnMouseDown()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //        
        //     }
        //     else if (Input.GetMouseButtonDown(1))
        //     {
        //        
        //     }
        // }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
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
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                IsSticky = !IsSticky;
                if (_extended)
                {
                    ((PistonArmTile) Workshop.GetTileAt(Position + _orientation)).IsSticky = IsSticky;
                }

                Workshop.RefreshTileRenderers();
            }
        }
    }
}