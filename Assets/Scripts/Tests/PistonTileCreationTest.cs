using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.Machineries;
using NUnit.Framework;
using UnityEngine;

namespace CatProcessingUnit.Tests
{
    public class PistonTileCreationTest
    {
        [Test]
        public void OnlyPiston()
        {
            var piston = new Piston(Vector2Int.one, Vector2Int.left, 1, 1);
            var positions = piston.GetTiles().Select(p => p.Item1).ToList();
            Assert.AreEqual(2, positions.Count);
            Assert.Contains(new Vector2Int(0, 1), positions);
            Assert.Contains(new Vector2Int(1, 1), positions);
        }
    }
}