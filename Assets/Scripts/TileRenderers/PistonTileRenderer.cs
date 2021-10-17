using System;
using CatProcessingUnit.TileDataNS;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit.TileRenderers
{
    public class PistonTileRenderer : RotatableTileRenderer, ICreateTileDataFromRenderer
    {
        [SerializeField] private SpriteConfig _sprites;
        [SerializeField] private int _maxLength;

        private void OnValidate()
        {
            if (_maxLength < 1)
            {
                _maxLength = 1;
            }
        }

        public void Render(PistonTileData pistonTileData)
        {
            base.Render(pistonTileData);
            var sticky = pistonTileData.Sticky;
            var extended = pistonTileData.Extended;
            _spriteRenderer.sprite = _sprites.GetPistonSprite(sticky, extended);
        }

        public TileData CreateData(Vector2Int position)
        {
            var piston = new PistonTileData(
                position,
                RotationUtil.RightVectorToDirection(transform.right),
                _maxLength);
            piston.SetRenderer(this);
            return piston;
        }
    }
}