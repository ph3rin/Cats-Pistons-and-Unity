using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit
{
    public static class WorkshopDataUtils
    {
        public static T FindCounterpart<T>(this WorkshopData data, T tile) where T : TileData
        {
            var result = data.GetTileAt(tile.Position) as T; 
            Debug.Assert(result != null, "result != null");
            return result;
        }
    }
}