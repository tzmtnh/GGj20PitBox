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

    private float _soundVolume = 100;

    public float GetVolume()
    {
        return _soundVolume;
    }

    public void SetVolume(float volume)
    {
        _soundVolume = volume;
    }
}
