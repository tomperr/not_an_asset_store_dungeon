using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
   

    // Update is called once per frame
    public Rigidbody2D projectile;
    public float time;
    public Transform Launcher;
    public GameObject HandR;
    public GameObject HandL;

    private GameObject fireballs;


    private List<Rigidbody2D> listeBoules;

    private float timer;
    

    public float projectileSpeed = 1f;

    // Use this for initialization
    void Start()
    {
        listeBoules = new List<Rigidbody2D>();
        fireballs = GameObject.Find("Fireballs");
    }

    private Vector2 ComputeVector(float x)
    {
        float rad = 2 * Mathf.PI * x;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    private List<Vector2> ComputeListVectors(int Count)
    {
        List<Vector2> directions = new List<Vector2>();
        for(int i = 0; i < Count; i++)
        {
            directions.Add(ComputeVector((float)i / (float)Count));
        }
        return directions;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > time )
        {
            List<Rigidbody2D> boulesFeu = new List<Rigidbody2D>();

            for(int i = 0; i < 16; i++)
            {
                Rigidbody2D projectileInstancier;
                projectileInstancier = Instantiate(projectile, Launcher.position, Launcher.rotation) as Rigidbody2D;
                projectileInstancier.gameObject.transform.SetParent(fireballs.transform);
                boulesFeu.Add(projectileInstancier);
            }

            List<Vector2> directions = ComputeListVectors(boulesFeu.Count);

            for(int i = 0; i < boulesFeu.Count; i++)
            {
                boulesFeu[i].AddForce(directions[i] * projectileSpeed * Time.deltaTime);
            }

            timer = 0.0f;
        }

        //normaliser les vecteurs (faut les créer avant)
            
    }
}
