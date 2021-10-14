using System;
using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    public class RestartHandler : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private float _timeRequiredToReset;
        private Workshop _workshop;
        private float _timeSincePressed;
        private bool _requireRelease;
        
        private void Start()
        {
            _workshop = ServiceLocator.GetService<Workshop>();
            _timeSincePressed = 0;
            _requireRelease = false;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                _requireRelease = false;
            }
            
            if (Input.GetKey(KeyCode.R) && !_requireRelease)
            {
                _timeSincePressed += Time.deltaTime;
                _progressBar.gameObject.SetActive(true);
                _progressBar.fillAmount = _timeSincePressed / _timeRequiredToReset;

                if (_timeSincePressed >= _timeRequiredToReset)
                {
                    _workshop.Restart();
                    _timeSincePressed = 0.0f;
                    _requireRelease = true;
                }
            }
            else
            {
                _timeSincePressed = 0;
                _progressBar.gameObject.SetActive(false);
            }
        }
    }
}