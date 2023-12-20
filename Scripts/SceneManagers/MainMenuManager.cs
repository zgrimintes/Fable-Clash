using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void FreePlay() //Start the free play mode
    {
        SceneManager.LoadScene("SampleScene");
        StartCoroutine(WaitAFrame());
    }

    IEnumerator WaitAFrame()
    {
        yield return 0;

        GameManager.Instance.updateGameState(GameStates.Start);
    }
}
