using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CArMenu : MonoBehaviour
{
    [SerializeField]
    private MainMenu _mm;
    public void StartGame()
    {
        _mm.PlayGame();
    }
}
