using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class Piston : Machinery
    {
        public Vector2Int Direction { get; }

        public Piston(Vector2Int position, Vector2Int direction) : base(position)
        {
            Direction = direction;
        }

        public Piston(Piston other) : base(other)
        {
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
            yield return (Vector2Int.zero, new Tile(this, TileSurface.Solid));
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