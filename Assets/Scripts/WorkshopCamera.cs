using System;
using UnityEngine;

namespace CatProcessingUnit
{
    public class WorkshopCamera : MonoBehaviour
    {
        [SerializeField] private Workshop _workshop;

        private void Awake()
        {
            var pos = transform.localPosition;
            pos.x = _workshop.Width * 0.5f - 0.5f;
            pos.y = _workshop.Height * 0.5f - 0.5f;
            transform.localPosition = pos;
        }
    }
}