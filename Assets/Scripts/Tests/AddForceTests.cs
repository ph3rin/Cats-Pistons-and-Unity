using System.Collections.Generic;
using CatProcessingUnit.Machineries;
using NUnit.Framework;
using UnityEngine;

namespace CatProcessingUnit.Tests
{
    public class AddForceTests
    {
        [Test]
        public void Pushing_OnePiston()
        {
            var pistonA = new Piston(new Vector2Int(1, 1), Vector2Int.right);
            var workspace = new Workspace(
                new[]
                {
                    pistonA
                }, 5, 5);
            Assert.IsTrue(workspace.ApplyForces(new List<Force>
            {
                new Force(pistonA, Vector2Int.down, Vector2Int.zero)
            }, null));
            Assert.IsNull(workspace.GetTileAt(1, 1));
            Assert.IsNotNull(workspace.GetTileAt(1, 0));
            Assert.AreSame(pistonA, workspace.GetTileAt(1, 0).Parent);
        }
        
        [Test]
        public void Pushing_OnePistonOutOfBounds()
        {
            var pistonA = new Piston(new Vector2Int(1, 0), Vector2Int.right);
            var workspace = new Workspace(
                new[]
                {
                    pistonA
                }, 5, 5);
            Assert.IsTrue(workspace.UpdateTiles());
            Assert.IsFalse(workspace.ApplyForces(new List<Force>
            {
                new Force(pistonA, Vector2Int.down, Vector2Int.zero)
            }, null));
        }

        [Test]
        public void Pushing_TwoPistons()
        {
            var pistonA = new Piston(new Vector2Int(1, 1), Vector2Int.right);
            var pistonB = new Piston(new Vector2Int(2, 1), Vector2Int.up);
            var workspace = new Workspace(
                new[]
                {
                    pistonA, pistonB
                }, 5, 5);
            Assert.IsTrue(workspace.ApplyForces(new List<Force>
            {
                new Force(pistonA, Vector2Int.right, Vector2Int.zero)
            }, null));
            Assert.IsNull(workspace.GetTileAt(1, 1));
            Assert.AreSame(pistonA, workspace.GetTileAt(2, 1).Parent);
            Assert.AreSame(pistonB, workspace.GetTileAt(3, 1).Parent);
        }
    }
}