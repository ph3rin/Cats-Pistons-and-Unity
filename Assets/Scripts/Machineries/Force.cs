using System.Collections.Generic;
using CatProcessingUnit.Machineries;
using UnityEngine;

namespace CatProcessingUnit
{
    public class Force
    {
        public Machinery Machinery { get; }
        public Vector2Int Direction { get; }
        public Vector2Int Position { get; }

        public Force(Machinery machinery, Vector2Int direction, Vector2Int position)
        {
            Machinery = machinery;
            Direction = direction;
            Position = position;
        }

        public IEnumerable<Force> Propagate(Workspace workspace)
        {
            return Machinery.PropagateForce(workspace, this);
        }

        public void Apply(Workspace workspace)
        {
            Machinery.ApplyForce(workspace, this);
        }
    }
}