using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Winscreen : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private void Start()
    {
        _text.text = "Your time was:/n" +PersistScript.PersistScriptInstance.GetTime();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
