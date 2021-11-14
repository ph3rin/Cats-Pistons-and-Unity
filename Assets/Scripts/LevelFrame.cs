using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit
{
    public class LevelFrame : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _renderers;

        public void ChangeDimensions(Vector2 center, Vector2 size)
        {
            transform.localPosition = center;
            foreach (var rdr in _renderers)
            {
                rdr.size = size + Vector2.one;
            }
        }
    }
}