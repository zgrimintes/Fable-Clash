using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMusicManager : MonoBehaviour
{
	public AudioSource src;
	public AudioClip biba;

	void Awake()
	{
		if (src.clip != biba)
		{
			DontDestroyOnLoad(gameObject);
			src.clip = biba;
			src.volume = .4f;
			src.Play();
		}
	}
}
