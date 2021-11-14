using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.Machineries;
using CatProcessingUnit.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CatProcessingUnit
{
    public enum GlobalState
    {
        UI,
        LevelSelection,
        Transition,
        GamePlay
    }

    [RequireComponent(typeof(RegisterService))]
    public class TransitionManager : MonoBehaviour, IService
    {
        [SerializeField] private float _levelTransitionTime;
        [SerializeField] private Transform _levelRootTransform;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _levelWidth;
        [SerializeField] private float _defaultCameraSize;
        [SerializeField] private float _levelSelectCameraSize;

        [FormerlySerializedAs("panelShowHide")] [FormerlySerializedAs("_levelSelectionShowHide")] [SerializeField]
        private ShowHidePanel _levelNumberPanel;

        [FormerlySerializedAs("_navBarShowHide")] [SerializeField]
        private ShowHidePanel _mainMenuBtn;

        [SerializeField] private Text _selectLevelBtnText;
        [SerializeField] private ShowHidePanel _selectLevelBtn;
        [SerializeField] private ParallaxCamera _parallaxCamera;

        private List<LevelHistory> _levels;
        private int _activeLevelIndex;
        [SerializeField] private float _fastLevelTransitionTime;

        public static TransitionManager I => ServiceLocator.GetService<TransitionManager>();

        public GlobalState State { get; set; }

        public void Init()
        {
            State = GlobalState.UI;
            _activeLevelIndex = 0;
            CreateLevels();
        }

        public void CreateLevels()
        {
            _levels =
                _levelRootTransform.GetComponentsInChildren<LevelHistory>(false)
                    .ToList();
            foreach (var level in _levels)
            {
                level.gameObject.SetActive(false);
            }
        }

        public void CollapseLevels()
        {
            var i = 0;
            foreach (var level in _levels)
            {
                level.gameObject.SetActive(false);
                level.transform.localPosition = CalculateLevelLocalPos(i) - CalculateLevelCenter(i);
                ++i;
            }

            _levels[_activeLevelIndex].gameObject.SetActive(true);
        }

        private Vector2 CalculateLevelCenter(int id)
        {
            return id * _levelWidth * Vector2.right;
        }

        private Vector2 CalculateLevelLocalPos(int id)
        {
            var level = _levels[id];
            var pos = CalculateLevelCenter(id);
            pos -= new Vector2(level.Width - 1.0f, level.Height - 1.0f) * 0.5f;
            return pos;
        }

        public void DistributeLevels()
        {
            var i = 0;
            foreach (var level in _levels)
            {
                var localPos = CalculateLevelLocalPos(i);
                level.transform.localPosition = localPos;
                level.gameObject.SetActive(true);
                ++i;
            }
        }

        public void TransitionToLevelSelection()
        {
            if (State == GlobalState.Transition) return;

            if (State == GlobalState.LevelSelection) return;

            State = GlobalState.Transition;
            CollapseLevels();
            InternalActiveLevel.gameObject.SetActive(true);

            IEnumerator Crt()
            {
                var seq = DOTween.Sequence();
                seq.Join(_camera.transform.DOMove(_levelRootTransform.CameraPos(), 2.0f));
                seq.Append(_camera.DOOrthoSize(_levelSelectCameraSize, 0.2f));
                seq.Append(_levelNumberPanel.Show());
                seq.Join(_mainMenuBtn.Show());
                seq.Join(_selectLevelBtn.Show());
                yield return seq.WaitForCompletion();
                DistributeLevels();
                _parallaxCamera.enabled = false;
                _camera.transform.position =
                    _levelRootTransform.CameraPos() + (Vector3) CalculateLevelCenter(_activeLevelIndex);
                _parallaxCamera.enabled = true;
                State = GlobalState.LevelSelection;
            }

            StartCoroutine(Crt());
        }

        public void TransitionToLevelSelectionFromGamePlay()
        {
            if (State == GlobalState.Transition) return;

            if (State == GlobalState.LevelSelection)
            {
                TransitionToGamePlay();
                return;
            }

            State = GlobalState.Transition;
            CollapseLevels();
            InternalActiveLevel.gameObject.SetActive(true);

            IEnumerator Crt()
            {
                _parallaxCamera.enabled = false;
                _camera.transform.position = _levelRootTransform.CameraPos();
                _parallaxCamera.enabled = true;
                var seq = DOTween.Sequence();
                seq.Append(_camera.DOOrthoSize(_levelSelectCameraSize, 0.2f));
                seq.Append(_levelNumberPanel.Show());
                seq.Join(_mainMenuBtn.Show());
                seq.Join(_levelNumberPanel.Hide());
                yield return seq.WaitForCompletion();
                DistributeLevels();
                _parallaxCamera.enabled = false;
                _camera.transform.position =
                    _levelRootTransform.CameraPos() + (Vector3) CalculateLevelCenter(_activeLevelIndex);
                _parallaxCamera.enabled = true;
                State = GlobalState.LevelSelection;
            }

            StartCoroutine(Crt());
        }

        public void TransitionToGamePlay()
        {
            Debug.Assert(State != GlobalState.Transition);

            if (State == GlobalState.GamePlay) return;

            State = GlobalState.Transition;
            _parallaxCamera.enabled = false;
            DistributeLevels();
            _camera.transform.position =
                _levelRootTransform.CameraPos() + (Vector3) CalculateLevelCenter(_activeLevelIndex);

            IEnumerator Crt()
            {
                yield return DOTween.Sequence()
                    .Join(_camera.DOOrthoSize(_defaultCameraSize, 0.2f))
                    .Join(_levelNumberPanel.Hide())
                    .Join(_mainMenuBtn.Show())
                    .Join(_selectLevelBtn.Show())
                    .WaitForCompletion();
                _parallaxCamera.enabled = true;
                State = GlobalState.GamePlay;
            }

            StartCoroutine(Crt());
        }

        public void TransitionToMainMenu()
        {
            if (State == GlobalState.Transition ||
                (State != GlobalState.GamePlay && State != GlobalState.LevelSelection)) return;

            var oldState = State;

            State = GlobalState.Transition;

            CollapseLevels();
            _parallaxCamera.enabled = false;
            _camera.transform.position = _levelRootTransform.CameraPos();
            _parallaxCamera.enabled = true;


            IEnumerator Crt()
            {
                var seq = DOTween.Sequence();
                if (oldState == GlobalState.LevelSelection)
                {
                    seq
                        .Append(_camera.DOOrthoSize(_defaultCameraSize, 0.2f))
                        .Join(_levelNumberPanel.Hide());
                }

                seq
                    .Append(_mainMenuBtn.Hide())
                    .Join(_selectLevelBtn.Hide())
                    .Join(_camera.transform.DOMove(Vector3.zero + Vector3.back * 10.0f, 2.0f));
                yield return seq.WaitForCompletion();
                State = GlobalState.UI;
            }

            StartCoroutine(Crt());
        }

        private LevelHistory InternalActiveLevel => _activeLevelIndex != -1 ? _levels[_activeLevelIndex] : null;

        public static LevelHistory ActiveLevel => ServiceLocator.GetService<TransitionManager>().InternalActiveLevel;
        public int ActiveLevelIndex => _activeLevelIndex;

        public IEnumerator ChangeLevelIndex(int delta)
        {
            if (State == GlobalState.Transition) yield break;
            var oldState = State;
            var oldLevel = ActiveLevel;
            _activeLevelIndex += delta;
            if (_activeLevelIndex < 0 || _activeLevelIndex >= _levels.Count)
            {
                _activeLevelIndex -= delta;
                yield break;
            }

            State = GlobalState.Transition;
            DistributeLevels();
            var finalCameraPos = _levelRootTransform.CameraPos() + (Vector3) CalculateLevelCenter(_activeLevelIndex);
            var seq = DOTween.Sequence();
            var transitionTime =
                oldState == GlobalState.LevelSelection ? _fastLevelTransitionTime : _levelTransitionTime;
            yield return DOTween.Sequence()
                .Join(_camera.transform.DOMove(finalCameraPos, transitionTime))
                .OnComplete(() => oldLevel.Restart(false))
                .WaitForCompletion();
            State = oldState;
        }

        public void NextLevel()
        {
            StartCoroutine(ChangeLevelIndex(1));
        }

        public void PreviousLevel()
        {
            StartCoroutine(ChangeLevelIndex(-1));
        }

        public IEnumerator TransitionToNextLevel()
        {
            return ChangeLevelIndex(1);
        }
    }
}