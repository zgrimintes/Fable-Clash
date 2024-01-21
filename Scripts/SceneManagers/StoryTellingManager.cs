using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTellingManager : MonoBehaviour
{
    public bool finishedAnimation = false;
    public int nextStoryInstance = 0;

    public void continueButton()
    {
        if (!finishedAnimation) return;

        nextStory(nextStoryInstance++);
    }

    public string nextStory(int i)
    {
        switch (i)
        {
            case 1:
                return "";
            default:
                return "N/A";
        }
    }
}
