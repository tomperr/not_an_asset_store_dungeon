using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_LH : MonoBehaviour {

    [SerializeField] GameObject follower; // ce sera le joueur normalement
    [SerializeField] float speed = 3.0f;

    private Rigidbody2D rb;
    private Vector2 room;

    public float damage;

    // Use this for initialization
    void Start () {
		
	}

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        follower = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update () {

        Vector2 position = rb.position;

        position += Direction() * speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(Vector3.forward * -1);

        rb.MovePosition(position);

    }

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
}
