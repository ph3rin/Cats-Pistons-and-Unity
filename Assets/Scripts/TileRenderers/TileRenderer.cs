using System;
using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit.TileRenderers
{
    public abstract class TileRenderer : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        public virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Debug.Assert(_spriteRenderer != null, "_spriteRenderer != null");
        }

        protected void Render(TileData tileData)
        {
            transform.localPosition = new Vector2(tileData.Position.x, tileData.Position.y);
        }
    }
}