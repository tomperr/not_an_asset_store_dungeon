using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Weapon : MonoBehaviour
{
    private GameObject owner;

    public float offset;
    public float range;
    public float damage;
    public float cooldown;
    public float timeLimit;

    private Quaternion angle = new Quaternion(0, 0, 6, 6);
    private Camera mainCamera;
    private Rigidbody2D rb;
    private float timer;
    private bool isDisable;
    private Vector2 direction;
    private Vector2 ecart;
    private Vector2 axisPosition;

    private SoundEffect se_hit;
    private SoundEffect se_death;

    private void Awake()
    {
        timer = 0.0f;
        rb = GetComponent<Rigidbody2D>();
        owner = GameObject.FindGameObjectWithTag("Player");
        gameObject.SetActive(false);
        isDisable = true;
        mainCamera = Camera.main;
        se_hit = GameObject.Find("SkeletonSound").GetComponent<SoundEffect>();
        se_death = GameObject.Find("SkeletonDeath").GetComponent<SoundEffect>();
    }

    void Update()
    {
        if (!isDisable) // si sur l'écran
        {
            rb.MovePosition((Vector2)(owner.transform.position) + ecart);
            timer += Time.deltaTime;
            if (timer > timeLimit)
            {
                timer = -0.0f;
                gameObject.SetActive(false);
                isDisable = true;
                ecart = Vector2.zero;
            }
        }

        if (isDisable)
            MoveWeapon();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // other est un ennemi
        if (other.gameObject.CompareTag("Mob"))
        {
            IAEnemy enemy = other.GetComponent<IAEnemy>();
            if (enemy.Current_hp - damage <= 0)
            {
                // death
                se_death.PlaySound();
            }
            else
            {
                // just hit
                se_hit.PlaySound();
                Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                Vector2 enemyPos = enemy.transform.position;

                Vector2 direction = enemyPos - playerPos;
                direction.Normalize();

                enemy.GetComponent<Rigidbody2D>().AddForce(direction * 0.2f, ForceMode2D.Impulse);
            }
            enemy.ChangeHealth(-damage);
        }
        if(other.gameObject.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            boss.ChangeHealth(-damage);
        }
    }

    public void Activate()
    {
        if (isDisable)
        {
            gameObject.SetActive(true);
            isDisable = false;

            if (Input.GetAxis("RightJoystickX") > 0.5 || Input.GetAxis("RightJoystickX") < -0.5 || Input.GetAxis("RightJoystickY") > 0.5 || Input.GetAxis("RightJoystickY") < -0.5)
            {
                MoveWeaponOnAxis(Input.GetAxis("RightJoystickX"), Input.GetAxis("RightJoystickY"));
            }
            else
            {
                MoveWeapon();
            }

            ecart = transform.position - owner.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void ChangeRange(float value)
    {
        this.range += value; 
    }

    public void ChangeDamage(float value)
    {
        this.damage += value;
    }

    public void ChangeCooldown(float value)
    {
        this.timeLimit *= value;
    }

    public void ChangeOffset(float value)
    {
        this.offset += value;
    }

    public void SetVisible(bool value)
    {
        gameObject.SetActive(value);
    }

    public void MoveWeapon()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 mouse_position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 player_position = owner.transform.position;
        direction = mouse_position - player_position;
        direction.Normalize();

        Vector3 maPosition = new Vector3(player_position.x + direction.x * offset, player_position.y + direction.y * offset, -5);
        
        transform.position = maPosition;
    }

    public void MoveWeaponOnAxis(float axeX, float axeY)
    {
        axisPosition.x = axeX;
        axisPosition.y = -axeY;
        //Vector3 mouse_position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 player_position = owner.transform.position;
        direction = axisPosition - player_position;
        direction.Normalize();

        Vector3 maPosition = new Vector3(player_position.x + direction.x * offset, player_position.y + direction.y * offset, -5);

        Debug.Log(axisPosition);

        transform.position = maPosition;
    }
}
