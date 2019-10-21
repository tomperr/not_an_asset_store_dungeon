using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bones : MonoBehaviour {

    GameObject player;
    GameObject enemy;
    private List<GameObject> bones;

    public int boneCount;
    public GameObject bone;
    public GameObject chair;
    public float degree;

    public GameObject Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < bones.Count; i++)
        {
            bones[i].GetComponent<Rigidbody2D>().velocity *= 0.9f;
            if (bones[i].GetComponent<Rigidbody2D>().velocity.magnitude <= 0.05f)
                bones[i].SetActive(false);
        }
	}

    private List<float> ComputeListAngles(int count, float degree)
    {
        List<float> list = new List<float>();
        float angleStep = degree / (float)count;
        float angle = angleStep/2;
        for (int i = 0; i < count; i++)
        {
            list.Add(angle);
            angle += angleStep;
        }
        return list;
    }

    public void Explode()
    {
        bones = new List<GameObject>();
        for (int i = 0; i < boneCount; i++)
        {
            if (enemy.gameObject.GetComponent<IAEnemy>().Max_hp == 5)
            {
                GameObject bonex = Instantiate(Resources.Load(bone.name)) as GameObject;
                bonex.transform.SetParent(transform);
                bones.Add(bonex);
            }
            if (enemy.gameObject.GetComponent<IAEnemy>().Max_hp == 3)
            {
                GameObject bonex = Instantiate(Resources.Load(chair.name)) as GameObject;
                bonex.transform.SetParent(transform);
                bones.Add(bonex);
            }
        }

        List<float> angles = ComputeListAngles(bones.Count, degree);
        player = GameObject.FindGameObjectWithTag("Player");
        float angle = Vector2.Angle(player.transform.position, enemy.transform.position);
        for (int i = 0; i < bones.Count; i++)
        {
            bones[i].SetActive(true);
            bones[i].GetComponent<Transform>().position = enemy.transform.position;

            bones[i].GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, angles[i]);
            bones[i].GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(90 + angle + angles[i]) * Random.Range(2f,25f), Mathf.Sin(90 + angle + angles[i]) * Random.Range(2f, 25f));
        }
    }

}
