using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterSelectManager : MonoBehaviour
{
    public static bool CH1 = true, CH2 = false, CH3 = false;

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void startChapetr1()
    {
        StoryTellingManager.story = true;
    }
}
