using System;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.Machineries;
using UnityEngine;

namespace CatProcessingUnit
{
    public class LevelCamera : MonoBehaviour
    {
        private void Start()
        {
            var workshop = ServiceLocator.GetService<LevelHistory>();
            var pos = transform.localPosition;
            pos.x = workshop.Width * 0.5f - 0.5f;
            pos.y = workshop.Height * 0.5f - 0.5f;
            transform.localPosition = pos;
        }
    }
}