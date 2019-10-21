using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{

    private AudioSource source;

	// Use this for initialization
	void Start ()
    {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PlaySound()
    {
        source.Play();
    }

    public void ChangeSound(AudioClip newClip)
    {
        source.clip = newClip;
    }

    public bool isPlaying()
    {
        return source.isPlaying;
    }

    public void StopSound()
    {
        source.Stop();
    }
}
