using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource src;
    public AudioClip virstango;
    public AudioClip biba;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (src.clip == null) src.clip = virstango;
        src.volume = .25f;
        src.Play();
    }

    public void onStartPlaying()
    {
        src.Pause();
    }
}
