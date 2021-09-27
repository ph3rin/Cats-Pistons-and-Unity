using System;
using UnityEngine;

namespace CatProcessingUnit
{
    public class TileFactory : MonoBehaviour
    {
        private static TileFactory _instance;

        public static TileFactory I => _instance;

        [SerializeField] private PistonArmTile _pistonArmPrefab;
        
        public PistonArmTile CreatePistonArm(Workshop workshop, Vector2Int orientation)
        {
            var arm = Instantiate(_pistonArmPrefab);
            arm.Workshop = workshop;
            arm.Orientation = orientation;
            return arm;
        }
        
        private void Awake()
        {
            _instance = this;
        }
    }
}