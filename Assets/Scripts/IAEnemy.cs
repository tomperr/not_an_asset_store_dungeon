using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEnemy : MonoBehaviour
{

    [SerializeField] GameObject follower; // ce sera le joueur normalement
    [SerializeField] float speed = 3.0f;
    [SerializeField] float max_hp = 10.0f;
    [SerializeField] float damage;

    private Rigidbody2D rb;
    public float current_hp;
    private Vector2 room;

    public float Max_hp
    {
        get { return max_hp; }
        set { max_hp = value; }
    }

    public float Current_hp
    {
        get { return current_hp;  }
        set { current_hp = value; }
    }

    public float Damage
    {
        get { return damage; }
        set { damage = value;  }
    }

    public Vector2 Room
    {
        get { return room; }
        set { room = value; }
    }

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        current_hp = max_hp;
        follower = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 position = rb.position;

        position += Direction() * speed * Time.deltaTime;
        position += rb.velocity;

        rb.velocity *= 0.90f;

        rb.MovePosition(position);
    }

    /// <summary>
    /// Renvoie un vecteur en direction du follower
    /// </summary>
    /// <returns>Le vecteur de direction vers le follower</returns>
    Vector2 Direction()
    {
        Vector2 direction = new Vector2();

        if (follower != null)
        {

            direction.x = follower.transform.position.x - transform.position.x;
            direction.y = follower.transform.position.y - transform.position.y;

            // si trop proche, on empeche le déplacement
            if (direction.x < 0.1 && direction.x > -0.1 && direction.y < 0.1 && direction.y > -0.1)
            {
                direction = Vector2.zero;
            }

            direction.Normalize();
        }

        return direction;
    }

    public void ChangeHealth(float amount)
    {
        if (amount < 0)
        {
            // damage
            // TODO : trigger animation
        }

        current_hp += amount;

        if (current_hp <= 0)
        {
            // TODO : Animation mort
            GameObject bones = Instantiate(Resources.Load("Bones")) as GameObject;
            bones.GetComponent<Bones>().Enemy = gameObject;
            bones.GetComponent<Bones>().Explode();
            gameObject.SetActive(false);
        }
    }

    
}




