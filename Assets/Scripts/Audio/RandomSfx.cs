using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace CatProcessingUnit.Audio
{
    [CreateAssetMenu(menuName = "SFX/Random")]
    public class RandomSfx : Sfx
    {
        [SerializeField] private List<AudioClip> _clips;
        [SerializeField] private float _baseVolume;
        [SerializeField] private float _volumeVariation;
        [SerializeField] private float _basePitch;
        [SerializeField] private float _pitchVariation;
        [SerializeField] private AudioMixerGroup _mixerGroup;
        
        private void OnValidate()
        {
            if (_baseVolume < 0.001) _baseVolume = 1.0f;
            if (_basePitch < 0.001) _basePitch = 1.0f;
        }

        public override AudioClip Clip =>
            _clips[Random.Range(0, _clips.Count)];

        public override float Volume =>
            _baseVolume + Random.Range(-_volumeVariation, _volumeVariation);

        public override float Pitch =>
            _basePitch + Random.Range(-_pitchVariation, _pitchVariation);

        public override AudioMixerGroup MixerGroup => _mixerGroup;
    }
}