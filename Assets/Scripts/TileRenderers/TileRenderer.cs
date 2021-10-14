using System;
using CatProcessingUnit.TileDataNS;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit.TileRenderers
{
    public class TileRenderer : MonoBehaviour, IPointerClickHandler
    {
        protected SpriteRenderer _spriteRenderer;
        public event Action onLeftClick;
        public event Action onRightClick;
        
        public virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Debug.Assert(_spriteRenderer != null, "_spriteRenderer != null");
        }

        public void Render(TileData tileData)
        {
            transform.localPosition = new Vector2(tileData.Position.x, tileData.Position.y);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onLeftClick?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                onRightClick?.Invoke();
            }
        }
    }
}