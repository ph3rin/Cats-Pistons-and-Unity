using System;
using System.Collections;
using UnityEngine;

namespace CatProcessingUnit.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class OneShotPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Play(Sfx sfx)
        {
            _audioSource.Stop();
            _audioSource.clip = sfx.Clip;
            _audioSource.pitch = sfx.Pitch;
            _audioSource.volume = sfx.Volume;
            _audioSource.Play();

            IEnumerator DestroyOnFinish()
            {
                yield return new WaitUntil(() => _audioSource.time >= 1.0f);
                Destroy(gameObject, 0.1f);
            }

            StartCoroutine(DestroyOnFinish());
        }
    }
}