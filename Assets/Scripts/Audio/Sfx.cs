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
            ServiceLocator.GetService<AudioManager>().CreatePlayer().Play(this);
        }
    }
}