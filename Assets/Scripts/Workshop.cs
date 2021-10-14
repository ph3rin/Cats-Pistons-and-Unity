using System;
using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.TileRenderers;
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

        private List<WorkshopData> _history;
        private int _activeDataIndex;

        public bool Solved => false;
        public WorkshopData ActiveData => _history[_activeDataIndex];
        public int Width => _width;
        public int Height => _height;

        public void Init()
        {
            _history = new List<WorkshopData>();
            _activeDataIndex = 0;
            GenerateTileGuides();
        }

        private void Start()
        {
            var initialData = TileBaker.BakeTiles(this);
            initialData.OnActivate();
            _history.Add(initialData);
        }

        private void GenerateTileGuides()
        {
            for (var x = 0; x < _width; ++x)
            {
                for (var y = 0; y < _height; ++y)
                {
                    var guide = Instantiate(_gridGuidePrefab, transform);
                    guide.transform.localPosition = new Vector2(x, y);
                }
            }
        }
        
        public bool Undo()
        {
            if (_activeDataIndex <= 0) return false;
            _history[_activeDataIndex].OnDeactivate();
            _history[--_activeDataIndex].OnActivate();
            return true;
        }
        
        public bool Redo()
        {
            if (_activeDataIndex >= _history.Count - 1) return false;
            _history[_activeDataIndex].OnDeactivate();
            _history[++_activeDataIndex].OnActivate();
            return true;
        }

        public void PushToHistory(WorkshopData data)
        {
            _history.RemoveRange(_activeDataIndex + 1, _history.Count - _activeDataIndex - 1);
            _history.Add(data);
            _history[_activeDataIndex].OnDeactivate();
            _history[++_activeDataIndex].OnActivate();
            RemoveUnusedRenderer();
        }

        private void RemoveUnusedRenderer()
        {
            foreach (var rdr in transform.GetComponentsInChildren<TileRenderer>(true))
            {
                if (ActiveData.Tiles.All(t => t.Renderer != rdr))
                {
                    Destroy(rdr.gameObject);
                }
            }
        }
    }
}