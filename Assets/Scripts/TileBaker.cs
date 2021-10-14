using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit
{
    public static class TileBaker
    {
        public static WorkshopData BakeTiles(Workshop workshop)
        {
            var workshopData = new WorkshopData(workshop);
            var width = workshop.Width;
            var height = workshop.Height;
            for (var i = 0; i < workshop.transform.childCount; ++i)
            {
                var child = workshop.transform.GetChild(i);
                if (!child.gameObject.activeInHierarchy) continue;
                var localPos = child.localPosition;
                var x = Mathf.RoundToInt(localPos.x);
                var y = Mathf.RoundToInt(localPos.y);
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    Debug.LogWarning("Tiles not within the predefined dimensions of this workshop grid", child);
                    continue;
                }

                if (!child.TryGetComponent(out ICreateTileDataFromRenderer creator)) continue;
                var tileData = creator.CreateData(new Vector2Int(x, y));
                workshopData.AddTile(tileData);
            }

            return workshopData;
        }
    }
}