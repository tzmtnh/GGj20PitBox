using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistScript : MonoBehaviour
{
    private static PersistScript _instance;
    public static PersistScript PersistScriptInstance
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
            DontDestroyOnLoad(gameObject);
        }
    }

    private float _soundVolume = 1;

    public float GetVolume()
    {
        return _soundVolume;
    }

    public void SetVolume(float volume)
    {
        _soundVolume = volume;
    }

    private string _time;

    public string GetTime()
    {
        return _time;
    }

    public void SetTime(string time)
    {
        _time = time;
    }
}
