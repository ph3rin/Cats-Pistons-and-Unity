using CatProcessingUnit.TileDataNS;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit.TileRenderers
{
    public class PistonTileRenderer : RotatableTileRenderer, ICreateTileDataFromRenderer
    {
        [SerializeField] private SpriteConfig _sprites;

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
                RotationUtil.RightVectorToDirection(transform.right));
            piston.SetRenderer(this);
            return piston;
        }
    }
}