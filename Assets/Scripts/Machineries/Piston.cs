using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class Piston : Machinery
    {
        private readonly Vector2Int _direction;

        public Piston(Vector2Int position, Vector2Int direction) : base(position)
        {
            _direction = direction;
        }

        public Piston(Piston other) : base(other)
        {
            _direction = other._direction;
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
            tile.Rotate(_direction);
            return tile;
        }

        private Vector2Int LocalToGlobalOffset(Vector2Int localOffset)
        {
            var right = _direction;
            var up = _direction.x * Vector2Int.up + _direction.y * Vector2Int.left;
            return localOffset.x * right + localOffset.y * up;
        }

        public override Machinery Clone()
        {
            return new Piston(this);
        }
    }
}