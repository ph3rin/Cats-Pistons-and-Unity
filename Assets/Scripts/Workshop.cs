using System;
using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatProcessingUnit
{
    [RequireComponent(typeof(RegisterService))]
    public class Workshop : MonoBehaviour, IService
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private GameObject _gridGuidePrefab;
        public bool Solved => false;

        [SerializeField] private List<Color> _targetColors;
        private List<Vector2Int> _targetPositions;

        private WorkshopTileData _tileData;

        public WorkshopTileData TileData => _tileData;

        public int Width => _width;
        public int Height => _height;

        public Color GetTargetColor(int targetIndex)
        {
            return _targetColors[targetIndex];
        }

        public Vector2Int GetTargetPosition(int targetIndex)
        {
            return _targetPositions[targetIndex];
        }
        
        public void Init()
        {
            foreach (var tile in transform.GetComponentsInChildren<WorkshopTile>())
            {
                tile.Workshop = this;
            }

            _targetPositions = new Vector2Int[_targetColors.Count].ToList();

            foreach (var target in transform.GetComponentsInChildren<TargetTile>())
            {
                var fPos = target.transform.localPosition;
                var pos = new Vector2Int(Mathf.RoundToInt(fPos.x), Mathf.RoundToInt(fPos.y));
                var idx = target.Index;
                target.SetColor(Color.white);
                _targetPositions[idx] = pos;
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
            Refresh();
        }

        public WorkshopTile GetTileAt(Vector2Int position)
        {
            return _tileData.GetTileAt(position);
        }

        public void SetData(WorkshopTileData data)
        {
            _tileData = data;
        }

        public void Refresh()
        {
            foreach (var tile in _tileData.Tiles)
            {
                var tileTr = tile.transform;
                tileTr.SetParent(transform);
                tileTr.localPosition = new Vector3(tile.Position.x, tile.Position.y, tileTr.localPosition.z);
                tile.RefreshDisplay();
            }

            var win = true;
            foreach (var tile in _tileData.Tiles)
            {
                if (tile is ColorTile colorTile)
                {
                    if (colorTile.Position != GetTargetPosition(colorTile.Index))
                    {
                        win = false;
                        break;
                    }
                }
            }

            if (win)
            {
                Debug.Log("You win!");
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}