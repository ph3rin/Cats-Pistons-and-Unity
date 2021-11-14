using System;
using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    [RequireComponent(typeof(Text))]
    public class LevelNumberText : MonoBehaviour
    {
        private Text _text;
        private TransitionManager _transitionManager;
        
        private void Start()
        {
            _text = GetComponent<Text>();
            _transitionManager = ServiceLocator.GetService<TransitionManager>();
        }

        private void Update()
        {
            _text.text = $"Level {_transitionManager.ActiveLevelIndex + 1:00}";
        }
    }
}