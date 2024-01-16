using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject gBasics1, gBasics2, gControls;

    public void GameBasics(int infos)
    {
        gBasics1.SetActive(1 == infos);
        gBasics2.SetActive(2 == infos);
    }
    public void Controls()
    {
        gControls.SetActive(true);
    }

    public void backToTutorial()
    {
        gBasics1.SetActive(false);
        gBasics2.SetActive(false);
        gControls.SetActive(false);
    }
}
