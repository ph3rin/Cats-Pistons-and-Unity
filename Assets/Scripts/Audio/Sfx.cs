using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.Audio;

namespace CatProcessingUnit.Audio
{
    public abstract class Sfx : ScriptableObject
    {
        public abstract AudioClip Clip { get; }
        public abstract float Volume { get; }
        public abstract float Pitch { get; }
        
        public abstract AudioMixerGroup MixerGroup { get; }
        
        public void Play()
        {
            Play(0.0f);
        }
        
        public void Play(float time)
        {
            ServiceLocator.GetService<AudioManager>().CreatePlayer().Play(this, time);
        }
    }
}