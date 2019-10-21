using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{

    [SerializeField] float hp;
    [SerializeField] float damage;
    [SerializeField] float vitesse;
    [SerializeField] float cooldown;
    [SerializeField] string message;
    [SerializeField] float range;
    [SerializeField] float offset;
    [SerializeField] float timeAppeared;

    private BonusTextUpdate textGui;
    private SoundEffect se;


    // Use this for initialization
    void Start ()
    {
        se = GameObject.Find("BonusSound").GetComponent<SoundEffect>();
        textGui = GameObject.Find("BonusText").GetComponent<BonusTextUpdate>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // collision avec le joueur
            PlayerSpecs playerSpecs = other.GetComponent<PlayerSpecs>();
            Weapon weapon = playerSpecs.dague.GetComponent<Weapon>();

          //  playerSpecs.AddBaseHP(hp);
            playerSpecs.AddHP(hp);
            playerSpecs.ChangeSpeed(vitesse);

            weapon.ChangeCooldown(0.8f);
            weapon.ChangeDamage(damage);
            weapon.ChangeOffset(offset);
            weapon.ChangeRange(range);
            textGui.BonusText.text = message;
            textGui.IsActive = true;

            se.PlaySound();

            // TODO : Animation ?

            Destroy(gameObject);
        }
    }
}
