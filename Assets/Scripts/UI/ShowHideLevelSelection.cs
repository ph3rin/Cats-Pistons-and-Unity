using System;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    [RequireComponent(typeof(Button))]
    public class ShowHideLevelSelection : MonoBehaviour
    {
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Text _actionText;
        [SerializeField] private string _hideText;
        [SerializeField] private string _showText;
        private RectTransform _myTransform;
        private Button _button;
        private bool _hidden;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _myTransform = GetComponent<RectTransform>();
            _button.onClick.AddListener(ToggleLevelSelectionPanel);
            _hidden = true;
        }

        private void ToggleLevelSelectionPanel()
        {
            if (_hidden)
            {
                _myTransform.anchoredPosition += Vector2.up * _panel.rect.height;
                _actionText.text = _hideText;
                _hidden = false;
            }
            else
            {
                _myTransform.anchoredPosition -= Vector2.up * _panel.rect.height;
                _actionText.text = _showText;
                _hidden = true;
            }
        }
    }
}