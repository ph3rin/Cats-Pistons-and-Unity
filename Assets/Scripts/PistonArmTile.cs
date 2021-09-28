using UnityEngine;

namespace CatProcessingUnit
{
    public class PistonArmTile : OrientedTile
    {
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _stickySprite;

        public bool IsSticky { get; set; }

        public override bool IsStickyOnOrientation(Vector2Int orientation)
        {
            return orientation == -_orientation || IsSticky && orientation == _orientation;
        }

        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            _spriteRenderer.sprite = IsSticky ? _stickySprite : _normalSprite;
        }
    }
}