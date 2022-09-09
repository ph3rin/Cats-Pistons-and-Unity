using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class Tile
    {
        public Machinery Parent { get; }
        public TileSurface Surface { get; private set; }

        public Tile(Machinery parent, TileSurface surface)
        {
            Parent = parent;
            Surface = surface;
        }

        public void Rotate(Vector2Int direction)
        {
            Surface = Surface.RotateTo(direction);
        }
    }
}