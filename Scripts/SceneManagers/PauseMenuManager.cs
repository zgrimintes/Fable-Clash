using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject infoP, infoE;

    bool showingInfo = true;

    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (!OffFinghtManager.Instance.game && !StoryTellingManager.story) return;

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

        if (OffFinghtManager.Instance.game && StoryTellingManager.story == false)
        {
            MusicManager.Instance.onStartPlaying();
            SceneManager.LoadScene("SampleScene");
            OffFinghtManager.Instance.startOfFight(true);
        }
        else
        {
            int storyInstance = StoryTellingManager.nextStoryInstance;
            if (storyInstance < 44) StoryTellingManager.nextStoryInstance = 0;
            else if (storyInstance < 68) StoryTellingManager.nextStoryInstance = 44;
            else StoryTellingManager.nextStoryInstance = 68;

            SceneManager.LoadScene("StoryTelling");
        }
    }

    public void onMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        OffFinghtManager.Instance.startOfFight(true);
    }

    public void onAbilitiesInfo()
    {
        infoE.SetActive(showingInfo);
        infoP.SetActive(showingInfo);

        showingInfo = !showingInfo;
    }
}
