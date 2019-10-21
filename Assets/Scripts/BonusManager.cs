using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{

    [SerializeField] List<GameObject> bonuses;

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    public GameObject GetRandomBonus()
    {
        int index = Random.Range(0, bonuses.Count);
        return bonuses[index];
    }

    public void SpawnRandomBonus(Vector2 position)
    {
        GameObject bonus = GetRandomBonus();
        SpawnSelectedBonus(position, bonus);
    }

    public GameObject SpawnSelectedBonus(Vector2 position, GameObject bonus)
    {
        Vector3 realPos = new Vector3(position.x, position.y, -1);
        GameObject instantiate = Instantiate(bonus, realPos, Quaternion.identity);
        return instantiate;
    }
}
