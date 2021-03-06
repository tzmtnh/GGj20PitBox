﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _fireExtinguisherAudioSource;
    [SerializeField] private AudioSource _fuelingAudioSource;
    [SerializeField] private AudioSource _electricRatchetAudioSource;
    [SerializeField] private AudioSource _carEngineAudioSource;
    [SerializeField] private AudioSource _engineFireAudioSource;
    [SerializeField] private AudioSource _enginePassiveSound;
	[SerializeField] private AudioSource _beepSound;

	[SerializeField] private AudioClip[] _electrictRatchetAudioClips;
    [SerializeField] private AudioMixer _audioMixer;

    private void Start()
    {
        _audioMixer.SetFloat("MusicVol", PersistScript.PersistScriptInstance.GetVolume());
    }

    public void PauseSound(bool pause)
    {
        if (pause)
        {
            _fireExtinguisherAudioSource.Pause();
            _fuelingAudioSource.Pause();
            _electricRatchetAudioSource.Pause();
            _carEngineAudioSource.Pause();
            _engineFireAudioSource.Pause();
            _enginePassiveSound.Pause();
            return;
        }
        _fireExtinguisherAudioSource.UnPause();
        _fuelingAudioSource.UnPause();
        _electricRatchetAudioSource.UnPause();
        _carEngineAudioSource.UnPause();
        _engineFireAudioSource.UnPause();
        _enginePassiveSound.UnPause();
        return;
    }

    private static AudioManager _instance;
    public static AudioManager AuidoManagerInstance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }


    //Engine Sound
    public void PlayingCarEngineAudio(bool isPlaying, float intensity)
    {
        _carEngineAudioSource.volume=intensity;
        if(isPlaying && !_carEngineAudioSource.isPlaying)
        {
            _carEngineAudioSource.Play();
        }
        else if(!isPlaying)
        {
            _carEngineAudioSource.Stop();
        }
    }


    //Fire Sound
    public void PlayingEngineFireAudio(bool isPlaying, float intensity)
    {
        _engineFireAudioSource.volume = intensity;
        if (isPlaying && !_engineFireAudioSource.isPlaying)
        {
            _engineFireAudioSource.Play();
        }
        else if (!isPlaying)
        {
            _engineFireAudioSource.Stop();
        }
    }

	public void PlayBeep(float pitch) {
		_beepSound.pitch = pitch;
		_beepSound.Stop();
		_beepSound.Play();
	}

    public bool IsPlayingFire()
    {
        return _engineFireAudioSource.isPlaying;
    }

    //Fueling Sound
    public void PlayingFuelingAudio(bool isPlaying, float intensity)
    {
        _fuelingAudioSource.volume = intensity;
        if (isPlaying && !_fuelingAudioSource.isPlaying)
        {
            _fuelingAudioSource.Play();
        }
        else if (!isPlaying)
        {
            _fuelingAudioSource.Stop();
        }
    }


    //Fire Extinguisher Sound
    public void PlayingFireExtinguisherAudio(bool isPlaying, float intensity)
    {
        _fireExtinguisherAudioSource.volume = intensity;
        if (isPlaying && !_fireExtinguisherAudioSource.isPlaying)
        {
            _fireExtinguisherAudioSource.Play();
        }
        else if (!isPlaying)
        {
            _fireExtinguisherAudioSource.Stop();
        }
    }


    //Ratchet Sound
    public void PlayOneShotRatchetAudio(float intensity)
    {
        _electricRatchetAudioSource.volume = intensity;
        _electricRatchetAudioSource.PlayOneShot(_electrictRatchetAudioClips[Random.Range(0,_electrictRatchetAudioClips.Length)]);
    }
}
