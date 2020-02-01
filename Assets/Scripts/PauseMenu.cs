using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuUI;
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Simulation.SimulationInst.IsGameRunning())
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

   public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Simulation.SimulationInst.Stop(false);
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Simulation.SimulationInst.Stop(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
