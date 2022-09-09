using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class Glass : Machinery, ICloneable<Glass>
    {
        public Glass(Vector2Int position) : base(position)
        {
        }

        public Glass(Machinery other) : base(other)
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

        public Glass Clone()
        {
            return new Glass(this);
        }
    }
}