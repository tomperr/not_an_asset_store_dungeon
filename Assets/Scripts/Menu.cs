using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    private SoundEffect se_select;
	// Use this for initialization
	void Start () {
        se_select = GameObject.Find("Select").GetComponent<SoundEffect>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        se_select.PlaySound();
        SceneManager.LoadScene("Default");
    }

    public void RetourMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
