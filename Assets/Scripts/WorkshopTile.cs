using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class WorkshopTile : MonoBehaviour
    {
        public Workshop Workshop { get; set; }
        public Vector2Int Position { get; set; }
        public abstract TileSurface Surface { get; }

        protected SpriteRenderer _spriteRenderer;
        
        protected void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void RefreshDisplay()
        {
        }

        public virtual bool IsStickyOnOrientation(Vector2Int orientation)
        {
            return false;
        }
    }
}