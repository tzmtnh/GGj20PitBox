using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    private void Start()
    {
        GetComponent<Slider>().value = PersistScript.PersistScriptInstance.GetVolume();
    }

    public void SetLevel(float SliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(SliderValue) * 20);
        PersistScript.PersistScriptInstance.SetVolume(Mathf.Log10(SliderValue)*20);
    }
}
