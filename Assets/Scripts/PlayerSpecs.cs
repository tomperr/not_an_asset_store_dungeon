using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpecs : MonoBehaviour {

    public float speed = 5f;
    public float baseHP = 5f;
    public GameObject dague;

    public float currentHP;
    private PlayerMotor pm;

    private SoundEffect se_weapon;
    private SoundEffect se_death;
    private CaveSound se_theme;


    public float CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    public float BaseHP
    {
        get { return baseHP; }
        set { baseHP = value; }
    }
    // Use this for initialization
    void Start ()
    {
        currentHP = baseHP;
        se_weapon = GameObject.Find("SwordSound").GetComponent<SoundEffect>();
        se_death = GameObject.Find("Death").GetComponent<SoundEffect>();
        se_theme = GameObject.Find("InGameTheme").GetComponent<CaveSound>();
    }

    // Update is called once per frame
    void Update () {
		if(IsAlive())
        {
            if(Input.anyKey)
            {
                if(Input.GetButton("Fire1"))
                {
                    // Arme -> Attaque somehow : prendre les multiplicateurs d'ici dans weapon ou les enlever ?
                    dague.GetComponent<Weapon>().Activate();
                    se_weapon.PlaySound();
                }
            }
            else if(Input.GetAxis("RightJoystickX") > 0.5 || Input.GetAxis("RightJoystickX") < -0.5 || Input.GetAxis("RightJoystickY") > 0.5 || Input.GetAxis("RightJoystickY") < -0.5)
            {
                dague.GetComponent<Weapon>().Activate();
                se_weapon.PlaySound();
            }
        }
        else
        {
            Die();
        }
	}

    public void AddHP(float value)
    {
        currentHP += value;
    }

    public void AddBaseHP(float value)
    {
        baseHP += value;
    }

    public bool IsAlive()
    {
        return currentHP > 0;
    }

    private void Die()
    {
        se_theme.Source.Stop();
        GameObject.Find("BossTheme").GetComponent<SoundEffect>().StopSound();
        se_death.PlaySound();
        gameObject.SetActive(false);
        SceneManager.LoadScene("Mort");
    }

    public void ChangeSpeed(float value)
    {
        this.speed += value;
    }
}
