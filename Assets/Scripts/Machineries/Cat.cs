using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class Cat : Machinery, ICloneable<Cat>
    {
        public Cat(Vector2Int position) : base(position)
        {
        }

        public Cat(Machinery other) : base(other)
        {
            
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
            yield return (Vector2Int.zero, new Tile(this, TileSurface.Solid));
        }

        public Cat Clone()
        {
            return new Cat(this);
        }
    }
}