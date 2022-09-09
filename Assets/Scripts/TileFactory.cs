using System;
using System.Data;
using CatProcessingUnit.TileDataNS;
using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit
{
    public class TileFactory : MonoBehaviour
    {
        private static TileFactory _instance;

        public static TileFactory I => _instance;

        [SerializeField] private PistonArmTileRenderer _pistonArmPrefab;

        public PistonArmTileRenderer CreatePistonArmRenderer(PistonArmTileData data)
        {
            var workshop = data.WorkshopData.Workshop;
            var arm = Instantiate(_pistonArmPrefab, workshop.transform);
            arm.transform.localPosition = (Vector2) data.Position;
            arm.transform.localRotation = RotationUtil.OrientationToRotation(data.Direction);
            return arm;
        }
        
        private void Awake()
        {
            _instance = this;
        }
    }
}