using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{

    public Sprite[] HeartSprites;

    public Image HeartUI;

    private GameObject player;

    private PlayerSpecs playerSpecs;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpecs = player.GetComponent<PlayerSpecs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HeartUI == null)
        {
            HeartUI = GameObject.Find("Heart").GetComponent<Image>();
        }
        HeartUI.sprite = HeartSprites[(int)playerSpecs.CurrentHP];
    }
}
