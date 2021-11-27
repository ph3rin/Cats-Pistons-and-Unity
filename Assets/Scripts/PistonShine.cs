using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CatProcessingUnit
{
    public class PistonShine : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _shineRenderer;
        [SerializeField] private Animator _shineAnimator;
        [SerializeField] private float _minInterval;
        [SerializeField] private float _maxInterval;
        private float _timeLeft;

        private void Start()
        {
            _timeLeft = Random.Range(_minInterval, _maxInterval);
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0.0f)
            {
                _timeLeft = Random.Range(_minInterval, _maxInterval);
                _shineRenderer.enabled = true;
                _shineAnimator.SetTrigger("Play");
            }
        }
    }
}