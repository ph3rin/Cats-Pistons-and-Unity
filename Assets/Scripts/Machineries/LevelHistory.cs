using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.Metrics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CatProcessingUnit.Machineries
{
    [RequireComponent(typeof(RegisterService))]
    public class LevelHistory : MonoBehaviour, IEnumerable<IMachineryHistory>
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private LevelFrame _levelFramePrefab;
        [SerializeField] private GameObject _gridGuidePrefab;
        [SerializeField] private UnityEvent _onRestart;
        private TransitionManager _transitionManager;
        
        private List<IMachineryHistory> _machineryHistories;
        [SerializeField] private bool _preventStickiness;
        
        public int Width => _width;
        public int Height => _height;
        public int ActiveIndex { get; private set; }
        public int HeadIndex { get; private set; }
        public int HistorySize { get; private set; }

        public bool PreventStickiness => _preventStickiness;
        
        public GameState State { get; private set; }

        public Vector2Int TargetPosition { get; private set; }

        private List<bool> _stability;

        private void Awake()
        {
            State = GameState.Gameplay;
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
            GenerateLevelFrame();
            FindTargetPosition();
        }

        private void Start()
        {
            _transitionManager = ServiceLocator.GetService<TransitionManager>();
            // MetricsManager.I.AddMetrics($"[LEVEL] Switch to level {ServiceLocator.GetService<LevelManager>().GetCurrentLevelId()}");
        }

        private void GenerateLevelFrame()
        {
            var frame = Instantiate(_levelFramePrefab, transform);
            var center = 0.5f * new Vector2(_width - 1, _height - 1);
            frame.ChangeDimensions(center, new Vector2(_width + 1, _height + 1));
        }

        private void FindTargetPosition()
        {
            var targetTile = GetComponentInChildren<TargetTileMarker>();
            if (targetTile != null)
            {
                TargetPosition = Vector2Int.RoundToInt(targetTile.transform.localPosition);
            }
            else
            {
                TargetPosition = new Vector2Int(-999, -999);
            }
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
            StabilizeHead(new AnimationOptions(0.18f));
        }

        public void StabilizeHead(AnimationOptions options)
        {
            if (HeadIndex == ActiveIndex) return;

            IEnumerator InternalStabilize()
            {
                State = GameState.Animation;
                var head = HeadIndex;
                var start = ActiveIndex;
                var sign = HeadIndex - ActiveIndex > 0 ? 1 : -1;
                for (int i = start; i != head; i += sign)
                {
                    var iLocal = i;
                    var coroutines = _machineryHistories.Select(
                            h => StartCoroutine(
                                h.MoveForward(iLocal, iLocal + sign, options)))
                        .ToList();
                    foreach (var coroutine in coroutines)
                    {
                        yield return coroutine;
                    }
                }

                var cat = _machineryHistories.Find(m => m is MachineryHistory<Cat>) as MachineryHistory<Cat>;
                if (cat != null)
                {
                    if (cat[ActiveIndex].Position == TargetPosition)
                    {
                        var catRenderer = transform.GetComponentInChildren<CatRenderer>();
                        State = GameState.UI;
                        Debug.Log("You win!");

                        IEnumerator Hack()
                        {
                            yield return catRenderer.Happy().WaitForCompletion();
                            yield return TransitionManager.I.TransitionToNextLevel(true);
                        }
                        yield return StartCoroutine(Hack());
                        yield break;
                    }
                }

                State = GameState.Gameplay;
            }

            StartCoroutine(InternalStabilize());

            _stability[HeadIndex] = true;
            ActiveIndex = HeadIndex;
        }

        public void Undo()
        {
            Debug.Assert(HeadIndex == ActiveIndex);
            if (State != GameState.Gameplay) return;
            if (ActiveIndex == 0)
            {
                return;
            }
            MetricsManager.I.AddMetrics("[HISTORY] Undo");
            do
            {
                --HeadIndex;
            } while (!_stability[HeadIndex]);

            StabilizeHead(new AnimationOptions(1 / 32f + 1 / 64f));
        }

        public void Redo()
        {
            Debug.Assert(HeadIndex == ActiveIndex);
            if (State != GameState.Gameplay) return;
            if (ActiveIndex >= HistorySize - 1)
            {
                return;
            }
            MetricsManager.I.AddMetrics("[HISTORY] Redo");
            do
            {
                ++HeadIndex;
            } while (!_stability[HeadIndex]);

            StabilizeHead(new AnimationOptions(1 / 32f + 1 / 64f));
        }

        public void Restart(bool invokeEvents = true)
        {
            if (HeadIndex == 0) return;
            MetricsManager.I.AddMetrics("[HISTORY] Reset");
            HeadIndex = 0;
            if (invokeEvents)
            {
                StabilizeHead(new AnimationOptions(0.5f / ActiveIndex));
            }
            else
            {
                StabilizeHead(AnimationOptions.Instant);
                HistorySize = 1;
            }
            if (invokeEvents)
            {
                _onRestart.Invoke();
            }
        }
    }

    public enum GameState
    {
        Gameplay,
        Animation,
        UI
    }
}