using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    public Vector2 coords;
    public Dictionary<string, Room> neighbors;

    public static int IndexCounter = 0;

    private int index;

    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    public float wallsize = 3.0f;

    public Room(int x, int y)
    {
        this.coords = new Vector2(x, y);
        this.neighbors = new Dictionary<string, Room>();
        enemies = new List<GameObject>();
        Index = IndexCounter;
        IndexCounter++;
    }

    public Room(Vector2 roomCoordinate)
    {
        this.coords = roomCoordinate;
        this.neighbors = new Dictionary<string, Room>();
        enemies = new List<GameObject>();
        Index = IndexCounter;
        IndexCounter++;
    }

    private int indexRoom;
    public int IndexRoom
    {
        get { return indexRoom; }
        set { indexRoom = value; }
    }

    private List<GameObject> enemies;
    public List<GameObject> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }

    private Boolean isBoss;
    public Boolean IsBoss
    {
        get { return isBoss; }
        set { isBoss = value; }
    }

    private GameObject bonus;
    public GameObject Bonus
    {
        get { return bonus; }
        set { bonus = value; }
    }


    public List<Vector2> NeighborCoordinates()
    {
        List<Vector2> neighborCoordinates = new List<Vector2>();
        neighborCoordinates.Add(new Vector2(this.coords.x, this.coords.y - 1));
        neighborCoordinates.Add(new Vector2(this.coords.x + 1, this.coords.y));
        neighborCoordinates.Add(new Vector2(this.coords.x, this.coords.y + 1));
        neighborCoordinates.Add(new Vector2(this.coords.x - 1, this.coords.y));

        return neighborCoordinates;
    }

    public void Connect(Room neighbor)
    {
        string direction = "";
        if (neighbor.coords.y < this.coords.y)
        {
            direction = "N";
        }
        if (neighbor.coords.x > this.coords.x)
        {
            direction = "E";
        }
        if (neighbor.coords.y > this.coords.y)
        {
            direction = "S";
        }
        if (neighbor.coords.x < this.coords.x)
        {
            direction = "W";
        }
        this.neighbors.Add(direction, neighbor);
    }

    public Room Neighbor(string direction)
    {
        return this.neighbors[direction];
    }

    public void AddMob(List<GameObject> enemyTemplates, int minEnemySpawn, int maxEnemySpawn, Vector2 roomSize, GameObject maRoom)
    {
        // spawn enemies
        
        int nb = UnityEngine.Random.Range(minEnemySpawn, maxEnemySpawn + 1);
        Vector3 position = new Vector3(0, 0, -5);
        Vector2 randomPos = Vector2.zero;
        bool isOk;

        maRoom.SetActive(true);

        for (int i = 0; i < nb; i++)
        {
            int index = UnityEngine.Random.Range(0, enemyTemplates.Count);
            do
            {
                isOk = true;
                position = enemyTemplates[index].transform.position;
                randomPos = GetEnemyPosition(enemyTemplates[index], roomSize);
                foreach (Transform child in maRoom.transform)
                {
                    Collider2D col = child.gameObject.GetComponent<Collider2D>();
                    col.bounds.Expand(col.bounds.size);
                    //Debug.Log("Size : " + col.bounds.size.ToString());
                    //Debug.Log(col.bounds);
                    if (col.bounds.Contains(randomPos))
                    {
                        // REFAIRE
                        //Debug.Log("JE RECOMMENCE MON SPAWN");
                        isOk = false;
                    }
                }
            } while (!isOk);

            position.x = randomPos.x;
            position.y = randomPos.y;

            GameObject newEnemy = UnityEngine.Object.Instantiate(enemyTemplates[index], position, Quaternion.identity);
            newEnemy.SetActive(false);

            if (newEnemy != null)
                Enemies.Add(newEnemy);
        }

        maRoom.SetActive(false);
    }

    public void SpawnMob()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject.GetComponent<IAEnemy>().Current_hp > 0)
                Enemies[i].SetActive(true);
        }
    }

    public void PrintMobs() // debug
    {
        String str = "";
        for (int i = 0; i < Enemies.Count; i++)
        {
            str += "Un enemy !\n";
        }
        Debug.Log(str);
    }

    public void RemoveMobs()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].SetActive(false);
        }
    }

    public void SetRandomRoom(int nbRooms)
    {
        indexRoom = UnityEngine.Random.Range(0, nbRooms);
    }

    Vector2 GetEnemyPosition(GameObject enemy, Vector2 size)
    {
        Vector2 position = new Vector2();

        Vector2 range_x = new Vector2(-size.x / 2 + wallsize, size.x / 2 - wallsize);
        Vector2 range_y = new Vector2(-size.y / 2 + wallsize, size.y / 2 - wallsize);

        position.x = UnityEngine.Random.Range(range_x.x, range_x.y);
        position.y = UnityEngine.Random.Range(range_y.x, range_y.y);

        // A MODIFIER

        return position;
    }
}

