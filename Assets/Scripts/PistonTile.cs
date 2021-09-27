using System;
using UnityEngine;

namespace CatProcessingUnit
{
    public class PistonTile : OrientedTile
    {
        [SerializeField] private Sprite _retractedSprite;
        [SerializeField] private Sprite _extendedSprite;

        [SerializeField] private bool _extended;

        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            _spriteRenderer.sprite = _extended ? _extendedSprite : _retractedSprite;
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

        private void OnMouseDown()
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