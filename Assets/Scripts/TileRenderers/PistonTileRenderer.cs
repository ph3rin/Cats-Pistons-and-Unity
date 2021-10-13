using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit.TileRenderers
{
    public class PistonTileRenderer : RotatableTileRenderer
    {
        [SerializeField] private SpriteConfig _sprites;

        public void Render(PistonTileData pistonTileData)
        {
            base.Render(pistonTileData);
            var sticky = pistonTileData.Sticky;
            var extended = pistonTileData.Extended;
            _spriteRenderer.sprite = _sprites.GetPistonSprite(sticky, extended);
        }
    }
}