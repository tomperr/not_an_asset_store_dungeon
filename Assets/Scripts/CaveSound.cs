using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSound : MonoBehaviour {

    // Use this for initialization

    private AudioSource source;

    public AudioSource Source
    {
        get { return source; }
        set { source = value; }
    }
	void Start ()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
        source.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
