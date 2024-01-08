using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            onPause();
        }
    }

    public void onPause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void onContinue()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void onRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
        OffFinghtManager.Instance.startOfFight();
    }

    public void onMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        OffFinghtManager.Instance.startOfFight();
    }
}
