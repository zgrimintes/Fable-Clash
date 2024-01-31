using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource src;
    public AudioClip virstango;
    public AudioClip ambientBackgroundMsc;
    public AudioClip biba;
    public AudioClip oRomanita;
    public AudioClip mioriticMetal;
    public AudioClip sirbaCaLaDociln;

    bool[] sceneChecked = new bool[5];
    int currScene;
    static float bibaTime = 0f;

    public void putOnTape(AudioClip tape)
    {
        src.clip = tape;
        src.volume = .2f;

        if (tape == biba)
        {
            Debug.Log(tape);
            src.time = bibaTime;
        }
        else src.time = 0;

        src.Play();
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        putOnTape(virstango);
    }

    private void Update()
    {
        checkScene();

        if (src.clip == biba)
        {
            bibaTime = src.time;
        }
    }

    public void checkScene()
    {
        currScene = SceneManager.GetActiveScene().buildIndex;


        if (sceneChecked[currScene] != false) return;

        if (currScene == 1 || currScene == 2) onStartPlaying();
        else if (currScene == 0) onMainMenu();
        if (currScene == 4) onStory();

        setSceneChecked(currScene);
    }

    public void setSceneChecked(int curr)
    {
        for (int i = 0; i < 5; i++)
            sceneChecked[i] = (curr == i);
    }

    public void onStartPlaying()
    {
        putOnTape(ambientBackgroundMsc);
    }

    public void onMainMenu()
    {
        if (src.clip != virstango)
            putOnTape(virstango);
    }

    public void onStory()
    {
        putOnTape(biba);
    }

    public void onCrisnicBattle()
    {
        putOnTape(sirbaCaLaDociln);
    }

    public void onStartFight()
    {
        float rand = Random.Range(0, 2);
        if (rand < 1) putOnTape(oRomanita);
        else if (rand <= 2) putOnTape(mioriticMetal);
    }
}
