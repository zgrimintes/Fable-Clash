using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCharacterSelMng : MonoBehaviour
{
    public AudioSource src;
    public AudioClip chSelBgMusic;
    public AudioClip oRomanita;
    public AudioClip mioriticMetal;

    void Start()
    {
        src.clip = chSelBgMusic;
        src.volume = .5f;
        src.Play();
    }

    public void onStartFight()
    {
        src.clip = chooseSong();
        src.volume = .25f;
        src.Play();
    }

    public AudioClip chooseSong()
    {
        float rand = Random.Range(0, 2);

        if (rand <= 1) return oRomanita;

        return mioriticMetal;
    }
}
