using System;
using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    [RequireComponent(typeof(Button))]
    public class LegacyNextLevelButton : MonoBehaviour
    {
        private Button _button;
        private LegacyLevelManager _legacyLevelManager;
        private int _levelIdx;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _legacyLevelManager = ServiceLocator.GetService<LegacyLevelManager>();
            _levelIdx = _legacyLevelManager.GetCurrentLevelIndex();
            if (!_legacyLevelManager.HasNextLevel())
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            _button.interactable = _legacyLevelManager.IsLevelCompleted(_levelIdx);
        }
        
        public void OnClick()
        {
            _legacyLevelManager.GoToNextLevel();
        }
    }
}