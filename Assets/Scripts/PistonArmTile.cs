using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit
{
    public class PistonArmTile : OrientedTile, IPointerClickHandler
    {
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _stickySprite;

        public bool IsSticky { get; set; }

        public override bool IsStickyOnOrientation(Vector2Int orientation)
        {
            return orientation == -_direction || IsSticky && orientation == _direction;
        }

        public override TileSurface Surface =>
            (IsSticky ? TileSurface.PistonArmSticky : TileSurface.PistonArm).RotateTo(_direction);

        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            _spriteRenderer.sprite = IsSticky ? _stickySprite : _normalSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var piston = Workshop.GetTileAt(Position - _direction) as PistonTile;
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                piston.ToggleExtension();
            }
            else
            {
                piston.ToggleStickiness();
            }
        }
    }
}