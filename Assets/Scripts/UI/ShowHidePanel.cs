using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    public class ShowHidePanel : MonoBehaviour
    {
        private enum MyState
        {
            Hidden,
            Animating,
            Shown
        }

        [SerializeField] private RectTransform _panel;
        [SerializeField] private int _direction;
        private RectTransform _myTransform;
        private MyState _myState;

        private void Awake()
        {
            _myTransform = GetComponent<RectTransform>();
            _myState = MyState.Hidden;
        }

        public Tween Show()
        {
            if (_myState == MyState.Animating || _myState == MyState.Shown)
            {
                return DOVirtual.DelayedCall(0, () => { });
            }

            _myState = MyState.Animating;
            var currentPos = _myTransform.anchoredPosition;

            return _myTransform
                .DOAnchorPos(currentPos + _direction * Vector2.up * _panel.rect.height, 0.25f)
                .OnComplete(() => _myState = MyState.Shown);
        }

        public Tween Hide()
        {
            if (_myState == MyState.Animating || _myState == MyState.Hidden)
            {
                return DOVirtual.DelayedCall(0, () => { });
            }

            _myState = MyState.Animating;
            var currentPos = _myTransform.anchoredPosition;

            return _myTransform
                .DOAnchorPos(currentPos -  _direction * Vector2.up * _panel.rect.height, 0.25f)
                .OnComplete(() => _myState = MyState.Hidden);
        }
    }
}