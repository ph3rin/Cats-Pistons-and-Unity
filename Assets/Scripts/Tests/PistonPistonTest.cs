using NUnit.Framework;
using UnityEngine;

namespace CatProcessingUnit.Tests
{
    public class PistonPistonTest
    {
        [Test]
        public void StickyPistonUp00_PistonRight01()
        {
            var a = TileSurface.PistonSticky.Rotate(1);
            var b = TileSurface.Solid;
            Assert.IsTrue(TileSurface.AreGluedTogether(Vector2Int.zero, a, Vector2Int.up, b));
        }
        
        [Test]
        public void PistonArmUp00_StickyPistonDown10()
        {
            var a = TileSurface.PistonArm.Rotate(1);
            var b = TileSurface.PistonSticky.Rotate(3);
            Assert.IsFalse(TileSurface.AreGluedTogether(Vector2Int.zero, a, Vector2Int.right, b));
        }
        
        [Test]
        public void PistonArmUp00_StickyPistonUp10()
        {
            var a = TileSurface.PistonArm.Rotate(1);
            var b = TileSurface.PistonSticky.Rotate(1);
            Assert.IsTrue(TileSurface.AreGluedTogether(Vector2Int.zero, a, Vector2Int.right, b));
        }

        [Test]
        public void StickyPistonArmUp00_StickyPistonArmDown10()
        {
            var a = TileSurface.PistonArmSticky.Rotate(1);
            var b = TileSurface.PistonArmSticky.Rotate(3);
            Assert.IsFalse(TileSurface.AreGluedTogether(Vector2Int.zero, a, Vector2Int.right, b));
        }
    }
}