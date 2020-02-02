using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Winscreen : MonoBehaviour
{
    [SerializeField] private Text _text;
    private void Start()
    {
        _text.text = "Your time was:\n" +PersistScript.PersistScriptInstance.GetTime();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
