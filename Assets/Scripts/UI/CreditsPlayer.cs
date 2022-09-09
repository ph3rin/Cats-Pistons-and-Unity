using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    public class CreditsPlayer : MonoBehaviour
    {
        [SerializeField] private float _interval;
        [SerializeField] private float _fadeTime;
        [SerializeField] private List<Text> _texts;

        private Coroutine _playCreditsCoroutine;

        private void Awake()
        {
            _playCreditsCoroutine = null;
        }

        public void StartPlaying()
        {
            foreach (var text in _texts)
            {
                text.gameObject.SetActive(false);
            }
            _playCreditsCoroutine = StartCoroutine(CrtPlay());
        }

        public void StopPlaying()
        {
            if (_playCreditsCoroutine == null) return;
            StopCoroutine(_playCreditsCoroutine);
            
            foreach (var text in _texts)
            {
                text.DOFade(0.0f, _fadeTime);
            }
            
            _playCreditsCoroutine = null;
        }

        public IEnumerator CrtPlay()
        {
            var index = 0;
            yield return FadeIn(_texts[0]);
            while (true)
            {
                yield return new WaitForSeconds(_interval);
                var oldIndex = index;
                var newIndex = (oldIndex + 1) % _texts.Count;

                var fadeOut = StartCoroutine(FadeOut(_texts[oldIndex]));
                var fadeIn = StartCoroutine(FadeIn(_texts[newIndex]));

                yield return fadeOut;
                yield return fadeIn;

                index = newIndex;
            }
        }

        private IEnumerator FadeIn(Text text)
        {
            text.gameObject.SetActive(true);
            text.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            yield return text.DOFade(1.0f, _fadeTime).WaitForCompletion();
        }

        private IEnumerator FadeOut(Text text)
        {
            yield return text.DOFade(0.0f, _fadeTime).WaitForCompletion();
            text.gameObject.SetActive(false);
        }
    }
}