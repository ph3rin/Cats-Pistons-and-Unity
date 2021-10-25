using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    [RequireComponent(typeof(RegisterService))]
    public class LevelHistory : MonoBehaviour, IEnumerable<IMachineryHistory>, IService
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private GameObject _gridGuidePrefab;

        private List<IMachineryHistory> _machineryHistories;
        public int Width => _width;
        public int Height => _height;
        public int ActiveIndex { get; private set; }
        public int HeadIndex { get; private set; }
        public int HistorySize { get; private set; }

        private List<bool> _stability;

        private void Awake()
        {
            _stability = new List<bool> {true};
            _machineryHistories = new List<IMachineryHistory>();
            ActiveIndex = 0;
            HeadIndex = 0;
            HistorySize = 1;
            foreach (var rdr in transform.GetComponentsInChildren<IMachineryRenderer>())
            {
                _machineryHistories.Add(rdr.CreateMachineryHistory(this));
            }

            GenerateTileGuides();
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

        public IEnumerator<IMachineryHistory> GetEnumerator()
        {
            return _machineryHistories.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this).GetEnumerator();
        }

        public void Push(List<IMachineryApplication> applications, AnimationOptions animationOptions = default)
        {
            foreach (var app in applications)
            {
                app.ApplyAt(HeadIndex + 1);
            }

            ++HeadIndex;
            HistorySize = HeadIndex + 1;
            while (_stability.Count < HistorySize)
            {
                _stability.Add(false);
            }

            _stability[HeadIndex] = false;
        }

        public void StabilizeHead()
        {
            if (HeadIndex == ActiveIndex) return;

            IEnumerator InternalStabilize()
            {
                var head = HeadIndex;
                var start = ActiveIndex;
                var sign = HeadIndex - ActiveIndex > 0 ? 1 : -1;
                for (int i = start; i != head; i += sign)
                {
                    var iLocal = i;
                    var coroutines = _machineryHistories.Select(
                        h => StartCoroutine(
                            h.MoveForward(iLocal, iLocal + sign, new AnimationOptions(0.125f))))
                        .ToList();
                    foreach (var coroutine in coroutines)
                    {
                        yield return coroutine;
                    };
                }
            }

            StartCoroutine(InternalStabilize());

            _stability[HeadIndex] = true;
            ActiveIndex = HeadIndex;
        }

        public void Init()
        {
        }

        public void Undo()
        {
            Debug.Assert(HeadIndex == ActiveIndex);
            if (ActiveIndex == 0)
            {
                return;
            }

            do
            {
                --HeadIndex;
            } while (!_stability[HeadIndex]);

            StabilizeHead();
        }

        public void Redo()
        {
            Debug.Assert(HeadIndex == ActiveIndex);
            if (ActiveIndex >= HistorySize - 1)
            {
                return;
            }

            do
            {
                ++HeadIndex;
            } while (!_stability[HeadIndex]);

            StabilizeHead();
        }
    }
}