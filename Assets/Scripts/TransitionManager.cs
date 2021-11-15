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
        GamePlay,
        Credits
    }

    [RequireComponent(typeof(RegisterService))]
    public class TransitionManager : MonoBehaviour, IService
    {
        [SerializeField] private float _creditsTransitionTime;
        [SerializeField] private AudioSource _titleAudio;
        [SerializeField] private AudioSource _gamePlayAudio;
        [SerializeField] private float _levelTransitionTime;
        [SerializeField] private Transform _levelRootTransform;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _levelWidth;
        [SerializeField] private float _defaultCameraSize;
        [SerializeField] private float _levelSelectCameraSize;
        [SerializeField] private ShowHidePanel _hintsPane;


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
        [SerializeField] private Transform _creditsCameraTransform;
        [SerializeField] private CreditsPlayer _creditsPlayer;

        public static TransitionManager I => ServiceLocator.GetService<TransitionManager>();

        public GlobalState State { get; set; }

        public void Init()
        {
            State = GlobalState.UI;
            _activeLevelIndex = 0;
            CreateLevels();
        }

        private void Awake()
        {
            _titleAudio.volume = 1.0f;
            _titleAudio.Play();
            // _titleAudio.DOFade(1.0f, 0.5f);
        }

        public Tween SwitchToTitleAudio()
        {
            if (_titleAudio.isPlaying) return DOVirtual.DelayedCall(0.0f, () => { });
            _titleAudio.volume = 0.0f;
            _titleAudio.Play();
            _titleAudio.time = 35f;
            return DOTween.Sequence()
                .Join(_titleAudio.DOFade(1.0f, 1.5f))
                .Join(_gamePlayAudio.DOFade(0.0f, 1.5f).OnComplete(() => _gamePlayAudio.Stop()));
        }

        public Tween SwitchToGamePlayAudio()
        {
            if (_gamePlayAudio.isPlaying) return DOVirtual.DelayedCall(0.0f, () => { });
            _gamePlayAudio.volume = 0.0f;
            _gamePlayAudio.Play();
            return DOTween.Sequence()
                .Join(_gamePlayAudio.DOFade(1.0f, 1.5f))
                .Join(_titleAudio.DOFade(0.0f, 1.5f).OnComplete(() => _titleAudio.Stop()));
        }

        public void TransitionToCreditsFromMainMenu()
        {
            if (State == GlobalState.Transition) return;

            State = GlobalState.Transition;

            DOTween.Sequence()
                .Join(_camera.transform.DOMove(_creditsCameraTransform.position, _creditsTransitionTime))
                .Join(_mainMenuBtn.Show())
                .OnComplete(() =>
                {
                    State = GlobalState.Credits;
                    _creditsPlayer.StartPlaying();
                });
        }

        public void TransitionToCreditsFromGameplay()
        {
            if (State == GlobalState.Transition) return;
            State = GlobalState.Transition;
            var oldLevel = ActiveLevel;
            CollapseLevels(_creditsCameraTransform.position.x);
            _parallaxCamera.enabled = false;
            _camera.transform.position =
                _levelRootTransform.CameraPos() + _creditsCameraTransform.position.x * Vector3.right;
            _parallaxCamera.enabled = true;

            DOTween.Sequence()
                .Join(_camera.transform.DOMove(_creditsCameraTransform.position, _creditsTransitionTime))
                .Join(_mainMenuBtn.Show())
                .Join(_selectLevelBtn.Hide())
                .Join(_hintsPane.Hide())
                .OnComplete(() =>
                {
                    oldLevel.Restart(false);
                    State = GlobalState.Credits;
                    _creditsPlayer.StartPlaying();
                    _activeLevelIndex = 0;
                });
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

        public void CollapseLevels(float horizontalOffset = 0.0f)
        {
            var i = 0;
            foreach (var level in _levels)
            {
                level.gameObject.SetActive(false);
                level.transform.localPosition = CalculateLevelLocalPos(i) - CalculateLevelCenter(i) +
                                                horizontalOffset * Vector2.right;
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
                seq.Join(SwitchToGamePlayAudio());
                seq.Join(_camera.transform.DOMove(_levelRootTransform.CameraPos(), 2.0f));
                seq.Append(_camera.DOOrthoSize(_levelSelectCameraSize, 0.2f));
                seq.Append(_levelNumberPanel.Show());
                seq.Join(_mainMenuBtn.Show());
                seq.Join(_selectLevelBtn.Show());
                seq.Join(_hintsPane.Hide());

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
                seq.Join(SwitchToGamePlayAudio());
                seq.Append(_camera.DOOrthoSize(_levelSelectCameraSize, 0.2f));
                seq.Append(_levelNumberPanel.Show());
                seq.Join(_mainMenuBtn.Show());
                seq.Join(_levelNumberPanel.Hide());
                seq.Join(_hintsPane.Hide());

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
                    .Join(SwitchToGamePlayAudio())
                    .Join(_camera.DOOrthoSize(_defaultCameraSize, 0.2f))
                    .Join(_hintsPane.Show())
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
                (State != GlobalState.GamePlay && State != GlobalState.LevelSelection &&
                 State != GlobalState.Credits)) return;

            var oldState = State;

            State = GlobalState.Transition;

            if (oldState != GlobalState.Credits)
            {
                CollapseLevels();
                _parallaxCamera.enabled = false;
                _camera.transform.position = _levelRootTransform.CameraPos();
                _parallaxCamera.enabled = true;
            }
            else
            {
                _creditsPlayer.StopPlaying();
            }


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
                    .Join(_hintsPane.Hide())
                    .Join(_selectLevelBtn.Hide())
                    .Join(_camera.transform.DOMove(Vector3.zero + Vector3.back * 10.0f, 2.0f));

                seq.Join(SwitchToTitleAudio());

                yield return seq.WaitForCompletion();
                State = GlobalState.UI;
            }

            StartCoroutine(Crt());
        }

        private LevelHistory InternalActiveLevel => _activeLevelIndex != -1 ? _levels[_activeLevelIndex] : null;

        public static LevelHistory ActiveLevel => ServiceLocator.GetService<TransitionManager>().InternalActiveLevel;
        public int ActiveLevelIndex => _activeLevelIndex;

        public IEnumerator ChangeLevelIndex(int delta, bool allowFinish = false)
        {
            if (State == GlobalState.Transition) yield break;
            var oldState = State;
            var oldLevel = ActiveLevel;
            _activeLevelIndex += delta;
            if (_activeLevelIndex < 0 || _activeLevelIndex >= _levels.Count)
            {
                if (_activeLevelIndex >= _levels.Count && allowFinish)
                {
                    _activeLevelIndex -= delta;
                    TransitionToCreditsFromGameplay();
                }
                else
                {
                    _activeLevelIndex -= delta;
                }

                yield break;
            }

            State = GlobalState.Transition;
            DistributeLevels();
            var finalCameraPos = _levelRootTransform.CameraPos() + (Vector3) CalculateLevelCenter(_activeLevelIndex);
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

        public IEnumerator TransitionToNextLevel(bool allowFinish = false)
        {
            return ChangeLevelIndex(1, allowFinish);
        }
    }
}