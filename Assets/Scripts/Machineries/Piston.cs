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

        public bool IsSticky { get; private set; }

        public Piston(Vector2Int position, Vector2Int direction, int maxLength = 1, int currentLength = 0,
            bool isSticky = false) :
            base(position)
        {
            Debug.Assert(maxLength >= 1);
            Debug.Assert(currentLength <= maxLength);

            Direction = direction;
            IsSticky = isSticky;
            MaxLength = maxLength;
            CurrentLength = currentLength;
        }

        public Piston(Piston other) : base(other)
        {
            IsSticky = other.IsSticky;
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
                var surface = IsSticky ? TileSurface.PistonSticky : TileSurface.Solid;
                yield return (Vector2Int.zero, new Tile(this, surface));
            }
            else
            {
                yield return (Vector2Int.zero, new Tile(this, TileSurface.PistonExtended));
                for (var i = 1; i < CurrentLength; ++i)
                {
                    yield return (Vector2Int.right * i, new Tile(this, TileSurface.PistonStem));
                }

                var surface = IsSticky ? TileSurface.PistonHeadSticky : TileSurface.PistonHead;
                yield return (Vector2Int.right * CurrentLength, new Tile(this, surface));
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
            if (CurrentLength == 0)
            {
                for (var i = 0; i < MaxLength; ++i)
                {
                    var (applications, piston) = history.SliceAt(levelHistory.HeadIndex);
                    var workspace = new Workspace(
                        applications.Select(app => app.Machinery),
                        levelHistory.Width,
                        levelHistory.Height);
                    if (piston.ExtendInternal(workspace))
                    {
                        levelHistory.Push(applications, new AnimationOptions(0.125f));
                    }
                }
                levelHistory.StabilizeHead();
            }
            else
            {
                for (var i = CurrentLength; i > 0; --i)
                {
                    var (applications, piston) = history.SliceAt(levelHistory.HeadIndex);
                    var workspace = new Workspace(
                        applications.Select(app => app.Machinery),
                        levelHistory.Width,
                        levelHistory.Height);
                    if (piston.RetractInternal(workspace))
                    {
                        levelHistory.Push(applications, new AnimationOptions(0.125f));
                    }
                }
                levelHistory.StabilizeHead();
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

        private bool RetractInternal(Workspace workspace)
        {
            Debug.Assert(CurrentLength >= 1);
            var headPos = Position + Direction * CurrentLength;
            var headTile = workspace.GetTileAt(headPos);
            var forces = new List<Force>();
            foreach (var delta in Workspace.Deltas)
            {
                var neighborPos = headPos + delta;
                var neighbor = workspace.GetTileAt(neighborPos);
                if (neighbor == null || ReferenceEquals(neighbor.Parent, this)) continue;
                if (TileSurface.AreGluedTogether(Vector2Int.zero, headTile.Surface, delta, neighbor.Surface))
                {
                    forces.Add(new Force(neighbor.Parent, -Direction, neighborPos - neighbor.Parent.Position));
                }
            }
            
            --CurrentLength;
            if (workspace.ApplyForces(forces, this))
            {
                if (workspace.UpdateTiles())
                {
                    return true;
                }
            }

            ++CurrentLength;
            return false;
        }

        public void SetStickiness(MachineryHistory<Piston> history, bool value)
        {
            var levelHistory = history.LevelHistory;
            var (applications, piston) = history.SliceAt(levelHistory.ActiveIndex);
            var machineries = applications.Select(a => a.Machinery).ToList();
            var oldStickyPiston = (Piston) machineries.FirstOrDefault(m => m is Piston {IsSticky: true});
            if (oldStickyPiston != null && !ReferenceEquals(piston, oldStickyPiston) && value)
            {
                oldStickyPiston.IsSticky = false;
            }

            piston.IsSticky = value;
            levelHistory.Push(applications, AnimationOptions.Instant);
            levelHistory.StabilizeHead();
        }
    }
}