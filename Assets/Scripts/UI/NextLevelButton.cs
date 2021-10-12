using System;
using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    [RequireComponent(typeof(Button))]
    public class NextLevelButton : MonoBehaviour
    {
        private Button _button;
        private Workshop _workshop;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _workshop = ServiceLocator.GetService<Workshop>();
        }

        private void Update()
        {
            _button.interactable = _workshop.Solved;
        }
    }
}