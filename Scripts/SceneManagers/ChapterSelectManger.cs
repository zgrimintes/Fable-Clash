using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterSelectManager : MonoBehaviour
{
    public static bool CH1 = true, CH2 = true, CH3 = true;
    public GameObject buttonCH1, buttonCH2, buttonCH3;
    public GameObject buttonCH2XBg, buttonCH3XBG;

    public void Start()
    {
        buttonCH1.GetComponent<Button>().enabled = CH1;
        buttonCH2.GetComponent<Button>().enabled = CH2; buttonCH2XBg.SetActive(!CH2);
        buttonCH3.GetComponent<Button>().enabled = CH3; buttonCH3XBG.SetActive(!CH3);
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void startChapetr1()
    {
        StoryTellingManager.story = true;
        StoryTellingManager.nextStoryInstance = 0;
        SceneManager.LoadScene("StoryTelling");
    }

    public void startChapetr2()
    {
        StoryTellingManager.story = true;
        StoryTellingManager.nextStoryInstance = 44;
        SceneManager.LoadScene("StoryTelling");
    }

    public void startChapetr3()
    {
        StoryTellingManager.story = true;
        StoryTellingManager.nextStoryInstance = 68;
        SceneManager.LoadScene("StoryTelling");
    }
}
