using System.Linq;
using CatProcessingUnit.Machineries;
using NUnit.Framework;
using UnityEngine;

namespace CatProcessingUnit.Tests
{
    public class WorkspaceTileTests
    {
        [Test]
        public void EmptyWorkspace()
        {
            var workspace = new Workspace(Enumerable.Empty<IMachineryApplication>(), 5, 5);
            Assert.IsTrue(workspace.UpdateTiles());
        }

        [Test]
        public void WorkspaceWithOnePiston()
        {
            var pistonA = new Piston(new Vector2Int(1, 4), Vector2Int.right);
            var workspace = new Workspace(
                new[]
                {
                    pistonA.NoOpApplication()
                }, 5, 5);
            Assert.IsTrue(workspace.UpdateTiles());
            Assert.AreSame(pistonA, workspace.GetTileAt(1, 4).Parent);
            Assert.AreEqual(TileSurface.Solid, workspace.GetTileAt(1, 4).Surface);
            Assert.IsNull(workspace.GetTileAt(6, 6));
            Assert.IsNull(workspace.GetTileAt(2, 6));
        }

        [Test]
        public void WorkspaceWithOnePiston_OutOfBounds()
        {
            var workspace = new Workspace(
                new[]
                {
                    new Piston(new Vector2Int(-1, 6), Vector2Int.right).NoOpApplication()
                }, 5, 5);
            Assert.IsFalse(workspace.UpdateTiles());
        }

        [Test]
        public void WorkspaceWithTwoPistons()
        {
            var workspace = new Workspace(
                new[]
                {
                    new Piston(new Vector2Int(0, 0), Vector2Int.right).NoOpApplication(),
                    new Piston(new Vector2Int(2, 1), Vector2Int.up).NoOpApplication(),
                }, 5, 5);
            Assert.IsTrue(workspace.UpdateTiles());
        }

        [Test]
        public void WorkspaceWithTwoPistons_Overlap()
        {
            var workspace = new Workspace(
                new[]
                {
                    new Piston(new Vector2Int(2, 1), Vector2Int.right).NoOpApplication(),
                    new Piston(new Vector2Int(2, 1), Vector2Int.up).NoOpApplication(),
                }, 5, 5);
            Assert.IsFalse(workspace.UpdateTiles());
        }

        [Test]
        public void WorkspaceWithTwoPistons_OutOfBounds()
        {
            var workspace = new Workspace(
                new[]
                {
                    new Piston(new Vector2Int(2, 1), Vector2Int.right).NoOpApplication(),
                    new Piston(new Vector2Int(5, 5), Vector2Int.up).NoOpApplication(),
                }, 5, 5);
            Assert.IsFalse(workspace.UpdateTiles());
        }
    }
}