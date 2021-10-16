using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit
{
    public class AnimationManager : MonoBehaviour, IService
    {
        private List<IEnumerator> _queuedAnimations;
        private List<Action> _animationFinishedCallbacks;
        
        private void Awake()
        {
            _queuedAnimations = new List<IEnumerator>();
            _animationFinishedCallbacks = new List<Action>();
        }

        public void Init()
        {
        }

        public void Queue(IEnumerator procedure)
        {
            _queuedAnimations.Add(procedure);
        }

        public AnimationManager PlayAll()
        {
            IEnumerator Crt()
            {
                var coroutines = _queuedAnimations.Select(StartCoroutine).ToList();
                foreach (var crt in coroutines)
                {
                    yield return crt;
                }

                foreach (var callback in _animationFinishedCallbacks)
                {
                    callback?.Invoke();
                }
                _queuedAnimations.Clear();
                _animationFinishedCallbacks.Clear();
            }

            StartCoroutine(Crt());
            return this;
        }

        public void AddAnimationFinishedCallback(Action callback)
        {
            _animationFinishedCallbacks.Add(callback);
        }
    }
}