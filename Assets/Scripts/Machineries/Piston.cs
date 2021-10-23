using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class Piston : Machinery
    {
        public Vector2Int Direction { get; }
        public int MaxLength { get; }
        public int CurrentLength { get; private set; }

        public Piston(Vector2Int position, Vector2Int direction, int maxLength = 1, int currentLength = 0) : base(position)
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
                for (var i = 1; i <= CurrentLength; ++i)
                {
                    // todo: distinguish between piston arm and head
                    yield return (Vector2Int.right * i, new Tile(this, TileSurface.PistonArm));
                }
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

        public override Machinery Clone()
        {
            return new Piston(this);
        }
    }
}