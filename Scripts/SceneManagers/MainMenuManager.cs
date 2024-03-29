using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void FreePlay() //Start the free play mode
    {
        StoryTellingManager.story = false;
        GameManager.Instance.updateGameState(GameStates.Start);
        SceneManager.LoadScene("SampleScene");
    }

    public void exitGame() //Close the game
    {
        Application.Quit();
    }

    public void StoryMode()
    {
        SceneManager.LoadScene("StoryMode");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}
