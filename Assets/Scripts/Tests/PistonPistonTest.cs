using NUnit.Framework;
using UnityEngine;

namespace CatProcessingUnit.Tests
{
    public class PistonPistonTest
    {
        [Test]
        public void StickyPistonUp00_PistonRight01()
        {
            var a = TileSurfacePresets.PistonSticky.Rotate(1);
            var b = TileSurfacePresets.Solid;
            Assert.IsTrue(TileSurface.AreGluedTogether(Vector2Int.zero, a, Vector2Int.up, b));
        }
        
        [Test]
        public void PistonArmUp00_StickyPistonDown10()
        {
            var a = TileSurfacePresets.PistonArm.Rotate(1);
            var b = TileSurfacePresets.PistonSticky.Rotate(3);
            Assert.IsFalse(TileSurface.AreGluedTogether(Vector2Int.zero, a, Vector2Int.right, b));
        }
        
        [Test]
        public void PistonArmUp00_StickyPistonUp10()
        {
            var a = TileSurfacePresets.PistonArm.Rotate(1);
            var b = TileSurfacePresets.PistonSticky.Rotate(1);
            Assert.IsTrue(TileSurface.AreGluedTogether(Vector2Int.zero, a, Vector2Int.right, b));
        }

        [Test]
        public void StickyPistonArmUp00_StickyPistonArmDown10()
        {
            var a = TileSurfacePresets.PistonArmSticky.Rotate(1);
            var b = TileSurfacePresets.PistonArmSticky.Rotate(3);
            Assert.IsFalse(TileSurface.AreGluedTogether(Vector2Int.zero, a, Vector2Int.right, b));
        }
    }
}