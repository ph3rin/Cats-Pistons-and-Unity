using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit
{
    public class SolidTile : WorkshopTile, IPointerClickHandler
    {
        public bool IsSticky { get; set; }

        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _stickySprite;

        public override TileSurface Surface { get; }

        public override void RefreshDisplay()
        {
            base.RefreshDisplay();
            _spriteRenderer.sprite = IsSticky ? _stickySprite : _normalSprite;
        }

        public override bool IsStickyOnOrientation(Vector2Int orientation)
        {
            return IsSticky;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right ||
                eventData.button == PointerEventData.InputButton.Left)
            {
                IsSticky = !IsSticky;
                Workshop.Refresh();
            }
        }
    }
}