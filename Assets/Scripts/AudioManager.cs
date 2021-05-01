using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundList[] sounds;

    private void Awake()
    {
        foreach(SoundList s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        SoundList s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogError("Sound" + name + "not found");
            return;
        }
        s.source.Play();
    }
}
