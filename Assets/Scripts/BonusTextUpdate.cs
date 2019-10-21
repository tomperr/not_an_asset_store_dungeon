using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BonusTextUpdate : MonoBehaviour {

    [SerializeField] float timeAppeared;

    private float timer;
    private bool isActive;

    private TMPro.TextMeshProUGUI bonusText;

    public TMPro.TextMeshProUGUI BonusText
    {
        get { return bonusText; }
        set { bonusText = value; }
    }

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    // Use this for initialization
    void Start ()
    {
        timer = 0.0f;
        isActive = false;
        bonusText = GetComponent<TMPro.TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer > timeAppeared)
            {
                timer = 0;
                isActive = false;
                bonusText.text = "";
            }
        }
    }
}
