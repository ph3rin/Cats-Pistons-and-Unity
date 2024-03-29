using UnityEngine.Audio;
using System;
using UnityEngine;

public class LegacyAudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static LegacyAudioManager I { get; private set; }
    
    void Awake()
    {
        I = this;
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
