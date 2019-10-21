using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitleSound : MonoBehaviour {

    private AudioSource audioSource;

    public AudioSource AudioSource
    {
        get { return audioSource; }
        set { audioSource = value; }
    }

	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.Play();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
}
