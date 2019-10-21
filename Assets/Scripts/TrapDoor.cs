using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject character = GameObject.Find("Character");
            DontDestroyOnLoad(character);

            GameObject sm = GameObject.Find("SoundManager");
            DontDestroyOnLoad(sm);

            GameObject igt = GameObject.Find("InGameTheme");
            DontDestroyOnLoad(igt);

            GameObject level = GameObject.FindGameObjectWithTag("Level");
            DontDestroyOnLoad(level);

            Destroy(GameObject.Find("DungeonGeneration"));
            character.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);

            SceneManager.LoadScene("Stage");
        }   
    }
}
