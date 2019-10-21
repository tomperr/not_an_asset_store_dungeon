using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerSpecs))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour
{
    private float speed;

    private Vector3 velocity;
    private Transform child;
    private Rigidbody2D rb;
    private Animator anim;
    private CharacterController charac;

    private SoundEffect se_hit;
    private SoundEffect se_walk;

    // Use this for initialization
    void Start()
    {
        child = transform.GetChild(0).transform;
        speed = GetComponent<PlayerSpecs>().speed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        charac = GetComponent<CharacterController>();

        se_hit = GameObject.Find("CharacterSound").GetComponent<SoundEffect>();
        se_walk = GameObject.Find("CharacterWalking").GetComponent<SoundEffect>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = Vector3.zero;
        if (Input.anyKey)
        {
            velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            anim.SetInteger("WalkState", 1);
            anim.SetFloat("Move X", Input.GetAxis("Horizontal"));
            anim.SetFloat("Move Y", Input.GetAxis("Vertical"));
        }
        else if (Input.GetAxis("LeftJoystickX") != 0 || Input.GetAxis("LeftJoystickY") != 0)
        {
            velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            anim.SetInteger("WalkState", 1);
            anim.SetFloat("Move X", Input.GetAxis("Horizontal"));
            anim.SetFloat("Move Y", Input.GetAxis("Vertical"));
        }
        else
        {
            anim.SetFloat("Move X", 0);
            anim.SetFloat("Move Y", 0);
            anim.SetInteger("WalkState", 0);
        }
        Vector3 newPosition = transform.position + velocity.normalized * speed * Time.deltaTime;

        if (velocity.magnitude > 0.1)
        {
            if (!se_walk.isPlaying())
            {
                se_walk.PlaySound();
            }
        }
        else
        {
            if (se_walk.isPlaying())
            {
                se_walk.StopSound();
            }
        }

        rb.MovePosition(newPosition);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mob")
        {
            IAEnemy enemy = collision.gameObject.GetComponent<IAEnemy>();
            if (enemy != null)
            {
                float damage = collision.gameObject.GetComponent<IAEnemy>().Damage;
                se_hit.PlaySound();
                GetComponent<PlayerSpecs>().AddHP(-damage);
                rb.velocity = Vector2.zero;
            }
        }
        if (collision.gameObject.tag == "Boss_Hand")
        {
            Pattern_LH main = collision.gameObject.GetComponent<Pattern_LH>();
            if (main != null)
            {
                float damage = collision.gameObject.GetComponent<Pattern_LH>().damage;
                se_hit.PlaySound();
                GetComponent<PlayerSpecs>().AddHP(-damage);
                rb.velocity = Vector2.zero;
            }
        }
        if (collision.gameObject.tag == "Fireball")
        {
            UneBoule uneBoule = collision.gameObject.GetComponent<UneBoule>();
            if (uneBoule != null)
            {
                float damage = collision.gameObject.GetComponent<UneBoule>().damage;
                se_hit.PlaySound();
                GetComponent<PlayerSpecs>().AddHP(-damage);
                rb.velocity = Vector2.zero;
            }
        }

    }
}
