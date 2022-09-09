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

        public void Play(Sfx sfx, float time = 0.0f)
        {
            _audioSource.Stop();
            _audioSource.clip = sfx.Clip;
            _audioSource.pitch = sfx.Pitch;
            _audioSource.volume = sfx.Volume;
            _audioSource.time = Mathf.Clamp(time, 0.0f, _audioSource.clip.length);
            _audioSource.Play();

            IEnumerator DestroyOnFinish()
            {
                yield return new WaitUntil(() => _audioSource.time >= _audioSource.clip.length);
                Destroy(gameObject, 0.3f);
            }

            StartCoroutine(DestroyOnFinish());
        }
    }
}