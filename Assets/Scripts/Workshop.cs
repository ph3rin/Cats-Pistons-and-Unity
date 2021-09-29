using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit
{
    public class Workshop : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private GameObject _gridGuidePrefab;

        [SerializeField] private List<Color> _targetColors;
        
        private WorkshopTileData _tileData;
        
        public WorkshopTileData TileData => _tileData;

        public Color GetTargetColor(int targetIndex)
        {
            return _targetColors[targetIndex];
        }
        
        private void Awake()
        {
            foreach (var tile in transform.GetComponentsInChildren<WorkshopTile>())
            {
                tile.Workshop = this;
            }

            for (var x = 0; x < _width; ++x)
            {
                for (var y = 0; y < _height; ++y)
                {
                    var guide = Instantiate(_gridGuidePrefab, transform);
                    guide.transform.localPosition = new Vector2(x, y);
                }
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