using UnityEngine;

namespace CatProcessingUnit
{
    public struct TileConnection
    {
        public WorkshopTile SrcTile { get; set; }
        public Vector2Int Offset { get; set; }

        public TileConnection(WorkshopTile srcTile, Vector2Int offset)
        {
            SrcTile = srcTile;
            Offset = offset;
        }
    }
}