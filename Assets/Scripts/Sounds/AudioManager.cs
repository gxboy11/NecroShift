using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    enum PlayTypes
    {
        MUSIC,
        SFX
    }

    [SerializeField]
    Sound[] musicSounds;

    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    Sound[] sfxSounds;

    [SerializeField]
    AudioSource sfxSource;

    void PlaySound(Sound[] sounds, AudioSource source, string name, PlayTypes playType)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound != null)
        {
            if (playType == PlayTypes.MUSIC)
            {
                source.clip = sound.sound;
                source.Play();
            }
            else
            {
                source.PlayOneShot(sound.sound);
            }
        }

    }

    public void PlayMusic(string name)
    {
        PlaySound(musicSounds, musicSource, name, PlayTypes.MUSIC);

    }

    public void PlaySFX(string name)
    {
        PlaySound(sfxSounds, sfxSource, name, PlayTypes.SFX);
    }
}
