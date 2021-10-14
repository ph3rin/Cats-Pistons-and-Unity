﻿using System;
using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    [RequireComponent(typeof(Button))]
    public class NextLevelButton : MonoBehaviour
    {
        private Button _button;
        private LevelManager _levelManager;
        private int _levelIdx;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _levelManager = ServiceLocator.GetService<LevelManager>();
            _levelIdx = _levelManager.GetCurrentLevelIndex();
            if (!_levelManager.HasNextLevel())
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            _button.interactable = _levelManager.IsLevelCompleted(_levelIdx);
        }
        
        public void OnClick()
        {
            _levelManager.GoToNextLevel();
        }
    }
}