using CatProcessingUnit.TileRenderers;
using UnityEngine;

namespace CatProcessingUnit.TileDataNS
{
    public abstract class TileData
    {
        public TileRenderer Owner { get; set; }
        public Vector2Int Position { get; set; }
        public abstract TileSurface Surface { get; }
    }
}