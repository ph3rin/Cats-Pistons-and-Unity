using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit.TileRenderers
{
    public class CatTileRenderer : TileRenderer, ICreateTileDataFromRenderer
    {
        public TileData CreateData(Vector2Int position)
        {
            return new CatTileData(position).SetRenderer(this);
        }
    }
}