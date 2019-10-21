using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_RH : MonoBehaviour
{

    private Rigidbody2D rb;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Vector2.up/10, ForceMode2D.Force);
        transform.Rotate(Vector3.forward * -1);
    }
}
