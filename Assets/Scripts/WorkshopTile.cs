using System;
using UnityEngine;

namespace CatProcessingUnit
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WorkshopTile : MonoBehaviour
    {
        public Workshop Workshop { get; set; }
        public Vector2Int Position { get; set; }

        protected SpriteRenderer _spriteRenderer;
        
        protected void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void RefreshDisplay()
        {
        }
    }
}