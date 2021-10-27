using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public abstract class Machinery
    {
        public Vector2Int Position { get; set; }
        public abstract IEnumerable<Force> PropagateForce(Workspace workspace, Force force);
        public abstract void ApplyForce(Workspace workspace, Force force);
        protected abstract IEnumerable<(Vector2Int, Tile)> GetTilesLocal();

        public IEnumerable<(Vector2Int, Tile)> GetTiles()
        {
            foreach (var (pos, tile) in GetTilesLocal())
            {
                yield return (pos + Position, tile);
            }
        }

        protected Machinery(Vector2Int position)
        {
            Position = position;
        }

        protected Machinery(Machinery other)
        {
            Position = other.Position;
        }
    }
}