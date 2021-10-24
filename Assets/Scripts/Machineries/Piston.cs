using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class Piston : Machinery, ICloneable<Piston>
    {
        public Vector2Int Direction { get; }
        public int MaxLength { get; }
        public int CurrentLength { get; private set; }

        public Piston(Vector2Int position, Vector2Int direction, int maxLength = 1, int currentLength = 0) :
            base(position)
        {
            Debug.Assert(maxLength >= 1);
            Debug.Assert(currentLength <= maxLength);

            Direction = direction;
            MaxLength = maxLength;
            CurrentLength = currentLength;
        }

        public Piston(Piston other) : base(other)
        {
            MaxLength = other.MaxLength;
            CurrentLength = other.CurrentLength;
            Direction = other.Direction;
        }

        public override IEnumerable<Force> PropagateForce(Workspace workspace, Force force)
        {
            return workspace.DefaultPropagateForce(force);
        }

        public override void ApplyForce(Workspace workspace, Force force)
        {
            Position += force.Direction;
        }

        protected override IEnumerable<(Vector2Int, Tile)> GetTilesLocal()
        {
            foreach (var (pos, tile) in GetTilesLocalUnrotated())
            {
                yield return (LocalToGlobalOffset(pos), CorrectRotation(tile));
            }
        }

        private IEnumerable<(Vector2Int, Tile)> GetTilesLocalUnrotated()
        {
            if (CurrentLength == 0)
            {
                yield return (Vector2Int.zero, new Tile(this, TileSurface.Solid));
            }
            else
            {
                yield return (Vector2Int.zero, new Tile(this, TileSurface.PistonExtended));
                for (var i = 1; i < CurrentLength; ++i)
                {
                    yield return (Vector2Int.right * i, new Tile(this, TileSurface.PistonStem));
                }

                yield return (Vector2Int.right * CurrentLength, new Tile(this, TileSurface.PistonHead));
            }
        }

        private Tile CorrectRotation(Tile tile)
        {
            tile.Rotate(Direction);
            return tile;
        }

        private Vector2Int LocalToGlobalOffset(Vector2Int localOffset)
        {
            var right = Direction;
            var up = Direction.x * Vector2Int.up + Direction.y * Vector2Int.left;
            return localOffset.x * right + localOffset.y * up;
        }

        public override Machinery CloneMachinery()
        {
            return Clone();
        }

        public Piston Clone()
        {
            return new Piston(this);
        }

        public void Extend(MachineryHistory<Piston> history)
        {
            var levelHistory = history.LevelHistory;
            var (applications, piston) = history.SliceAt(levelHistory.ActiveIndex);
            var workspace = new Workspace(
                applications.Select(app => app.Machinery),
                levelHistory.Width,
                levelHistory.Height);
            if (piston.ExtendInternal(workspace))
            {
                levelHistory.Push(applications);
            }
        }

        private bool ExtendInternal(Workspace workspace)
        {
            var headPos = Position + Direction * CurrentLength;
            var headTile = workspace.GetTileAt(headPos);
            var forces = new List<Force>();
            foreach (var delta in Workspace.Deltas)
            {
                var neighborPos = headPos + delta;
                var neighbor = workspace.GetTileAt(neighborPos);
                if (neighbor == null || ReferenceEquals(neighbor.Parent, this)) continue;
                if (delta == Direction ||
                    TileSurface.AreGluedTogether(Vector2Int.zero, headTile.Surface, delta, neighbor.Surface))
                {
                    forces.Add(new Force(neighbor.Parent, Direction, neighborPos - neighbor.Parent.Position));
                }
            }

            if (workspace.ApplyForces(forces, this))
            {
                ++CurrentLength;
                if (workspace.UpdateTiles())
                {
                    return true;
                }
            }

            return false;
        }
    }
}