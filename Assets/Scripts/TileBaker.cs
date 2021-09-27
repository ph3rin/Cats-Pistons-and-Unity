using UnityEngine;

namespace CatProcessingUnit
{
    public static class TileBaker
    {
        public static WorkshopTile[,] BakeTiles(Transform parent, int width, int height)
        {
            var tiles = new WorkshopTile[width, height];

            for (var i = 0; i < parent.childCount; ++i)
            {
                var child = parent.GetChild(i);
                var localPos = child.localPosition;
                var x = Mathf.RoundToInt(localPos.x);
                var y = Mathf.RoundToInt(localPos.y);
                if (!child.TryGetComponent(out WorkshopTile tile)) continue;
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    Debug.LogWarning("Tiles not within the predefined dimensions of this workshop grid", child);
                    continue;
                }

                tile.Position = new Vector2Int(x, y);
                tiles[x, y] = tile;
            }

            return tiles;
        }
    }
}