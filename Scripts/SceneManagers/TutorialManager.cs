using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject gBasics1, gBasics2, gControls, UI, storyP;

    public void GameBasics(int infos)
    {
        gBasics1.SetActive(1 == infos);
        gBasics2.SetActive(2 == infos);
    }
    public void Controls()
    {
        gControls.SetActive(true);
    }

    public void showUI()
    {
        UI.SetActive(true);
    }

    public void showStoryProgression()
    {
        storyP.SetActive(true);
    }

    public void backToTutorial()
    {
        gBasics1.SetActive(false);
        gBasics2.SetActive(false);
        gControls.SetActive(false);
        UI.SetActive(false);
        storyP.SetActive(false);
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
