using System.Collections;
using CatProcessingUnit.GameManagement;
using DG.Tweening;
using UnityEngine;

namespace CatProcessingUnit.UI
{
    [RequireComponent(typeof(RegisterService))]
    public class VictoryScreen : MonoBehaviour, IService
    {
        [SerializeField] private float _fadeInTime;
        [SerializeField] private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        public Tweener FadeIn()
        {
            _canvas.gameObject.SetActive(true);
            _canvasGroup.alpha = 0.0f;
            return _canvasGroup.DOFade(1.0f, _fadeInTime);
        }

        public void Init()
        {
            if (!_canvas.TryGetComponent(out _canvasGroup))
            {
                _canvasGroup = _canvas.gameObject.AddComponent<CanvasGroup>();
            }

            _canvasGroup.alpha = 0.0f;
            _canvas.gameObject.SetActive(false);
        }
    }
}