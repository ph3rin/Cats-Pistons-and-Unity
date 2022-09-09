using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit
{
    public interface ICreateTileDataFromRenderer
    {
        public TileData CreateData(Vector2Int position);
    }
}