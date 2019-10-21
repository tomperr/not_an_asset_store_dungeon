using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{

    public float max_hp;
    public float current_hp;
    private int damage;
    private Camera camera;
    private GameObject player;

    private Sprite[] sprites;
    private SpriteRenderer spriteR;
    private int spriteVersion = 0;

    public Slider healthBar;

    public GameObject mainD;
    public GameObject mainG;
    public GameObject launcher1;
    public GameObject launcher2;
    public GameObject launcher3;
    public GameObject mainG2;
    public GameObject mainD2;
    public GameObject mainMiddle;

    private SoundEffect se_boss;

    private GameObject trapDoor;

    bool enrage = false;

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        player = GameObject.Find("Character");
        player.transform.position = new Vector3(0.5f, -22, -5);
        player.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);

        spriteR = gameObject.GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("spritesheet_boss");
        trapDoor = GameObject.FindGameObjectWithTag("TrapDoor");
        trapDoor.SetActive(false);
        se_boss = GameObject.Find("BossTheme").GetComponent<SoundEffect>();
        se_boss.PlaySound();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = current_hp;
        Vector3 posCamera = new Vector3(camera.transform.position.x, player.transform.position.y, camera.transform.position.z);
        camera.transform.position = posCamera;

        if (current_hp <= max_hp / 2 && !enrage)
        {
            Enrage();
            enrage = true;
        }
        if (current_hp <= 0)
            Die();
    }

    void Enrage()
    {
        mainG.SetActive(true);
        mainD.SetActive(true);
        launcher1.SetActive(true);
        launcher2.SetActive(true);
        mainG2.SetActive(false);
        mainD2.SetActive(false);

        spriteR.sprite = sprites[3];
    }

    public void ChangeHealth(float amount)
    {
        if (amount < 0 && current_hp > 0)
        {
            // damage
            StartCoroutine(Damage());
        }

        current_hp += amount;

        if (current_hp <= 0)
        {
            trapDoor.SetActive(true);
        }
    }

    IEnumerator Damage()
    {
        spriteR.sprite = sprites[7];

        yield return new WaitForSeconds(1);

        if (enrage)
            spriteR.sprite = sprites[3];
        else
            spriteR.sprite = sprites[0];
    }

    public void Die()
    {
        mainG.SetActive(false);
        mainD.SetActive(false);
        launcher1.SetActive(false);
        launcher2.SetActive(false);
        launcher3.SetActive(false);
        mainMiddle.SetActive(false);
        spriteR.sprite = sprites[6];
        se_boss.StopSound();
    }
}
