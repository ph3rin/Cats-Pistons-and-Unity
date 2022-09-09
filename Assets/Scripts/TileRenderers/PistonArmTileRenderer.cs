using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit.TileRenderers
{
    public class PistonArmTileRenderer : RotatableTileRenderer
    {
        [SerializeField] private SpriteConfig _sprites;

        public void Render(PistonArmTileData tileData)
        {
            base.Render(tileData);
            _spriteRenderer.sprite = _sprites.GetPistonArmSprite(tileData.Sticky, tileData.IsStem);
        }
        
    }
}