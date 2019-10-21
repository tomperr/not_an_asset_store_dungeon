using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EchapMenu : MonoBehaviour {

    public GameObject PauseUI;

    private bool paused = false;
    void Start()
    {
        PauseUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            paused = !paused;

            if (paused)
            {
                PauseUI.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                PauseUI.SetActive(false);
                Time.timeScale = 1;
            }
        }  
    }

    public void Resume()
    {
        paused = false;
        PauseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Destroy(GameObject.Find("Character"));
        Destroy(GameObject.Find("Level"));
        SceneManager.LoadScene("Default");
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGenerator>().Init();
    }
}
