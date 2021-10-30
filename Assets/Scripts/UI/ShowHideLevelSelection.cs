using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    [RequireComponent(typeof(Button))]
    public class ShowHideLevelSelection : MonoBehaviour
    {
        private enum MyState
        {
            Hidden,
            Animating,
            Shown
        }
        
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Text _actionText;
        [SerializeField] private string _hideText;
        [SerializeField] private string _showText;
        private RectTransform _myTransform;
        private Button _button;
        private MyState _myState;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _myTransform = GetComponent<RectTransform>();
            _button.onClick.AddListener(ToggleLevelSelectionPanel);
            _myState = MyState.Hidden;
        }

        private void ToggleLevelSelectionPanel()
        {
            var currentPos = _myTransform.anchoredPosition;
            if (_myState == MyState.Hidden)
            {
                _myTransform
                    .DOAnchorPos(currentPos + Vector2.up * _panel.rect.height, 0.25f)
                    .OnStart(() => _myState = MyState.Animating)
                    .OnComplete(() => _myState = MyState.Shown);
                _actionText.text = _hideText;
            }
            else if (_myState == MyState.Shown)
            {
                _myTransform
                    .DOAnchorPos(currentPos - Vector2.up * _panel.rect.height, 0.25f)
                    .OnStart(() => _myState = MyState.Animating)
                    .OnComplete(() => _myState = MyState.Hidden);
                _actionText.text = _showText;
            }
        }
    }
}