using System;
using UnityEngine;

namespace CatProcessingUnit
{
    public class Workshop : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        private WorkshopTileData _tileData;

        public WorkshopTileData TileData => _tileData;

        private void Awake()
        {
            foreach (var tile in transform.GetComponentsInChildren<WorkshopTile>())
            {
                tile.Workshop = this;
            }
        }

        private void Start()
        {
            _tileData = new WorkshopTileData(TileBaker.BakeTiles(transform, _width, _height));
            RefreshTileRenderers();
        }

        public WorkshopTile GetTileAt(Vector2Int position)
        {
            return _tileData.GetTileAt(position);
        }

        public void SetData(WorkshopTileData data)
        {
            _tileData = data;
        }

        public void RefreshTileRenderers()
        {
            foreach (var tile in _tileData.Tiles)
            {
                var tileTr = tile.transform;
                tileTr.SetParent(transform);
                tileTr.localPosition = new Vector3(tile.Position.x, tile.Position.y, tileTr.localPosition.z);
                tile.RefreshDisplay();
            }
        }
    }
}