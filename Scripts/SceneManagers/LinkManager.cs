using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkManager : MonoBehaviour
{
    public void openEAnNa()
    {
        Application.OpenURL("https://e-an-na.com");
    }

    public void openGameDevAcademy()
    {
        Application.OpenURL("https://www.gamedevacademy.ro");
    }

    public void onpeWOU()
    {
        Application.OpenURL("https://www.worldofus.info/wou-game");
    }

    public void openTOU()
    {
        Application.OpenURL("https://talesofus.com");
    }

    public void onMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
