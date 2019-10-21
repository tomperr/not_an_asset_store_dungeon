using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets;

public class DungeonGenerator : MonoBehaviour {

    private static DungeonGenerator instance = null;

    public List<GameObject> enemyTemplates;
    public float tauxSpawn;
    public int minEnemySpawn;
    public int maxEnemySpawn;

    public List<GameObject> roomTemplates;
    private List<GameObject> roomScenes;

    [SerializeField]
    private int numberOfRooms;
    public Room[,] rooms;
    private Room currentRoom;

    public GameObject NDoor;
    public GameObject EDoor;
    public GameObject WDoor;
    public GameObject SDoor;

    public Room[,] Rooms
    {
        get { return rooms; }
        set { rooms = value; }
    }

    public int NumberOfRooms
    {
        get { return numberOfRooms; }
        private set { numberOfRooms = value; }
    }


    // Game Instance Singleton
    public static DungeonGenerator Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        roomScenes = new List<GameObject>();
        InitRoomsScene();
        if (instance == null)
        {
            //DontDestroyOnLoad(this.gameObject);
            instance = this;
            this.currentRoom = GenerateDungeon();
        }
        else
        {
            Destroy(this.gameObject);
        }
        Init();
    }

    void Start()
    {
    }

    public void Init()
    {
        this.currentRoom = GenerateDungeon();
        SetBossRoom();
        GenerateDoors();
        SetRoomBonus();
        currentRoom.IndexRoom = 0;
        roomScenes[CurrentRoom().IndexRoom].SetActive(true);
        GameObject.FindGameObjectWithTag("Minimap").GetComponent<Minimap>().Init();

        int number = int.Parse(GameObject.Find("Number").GetComponent<TMPro.TextMeshProUGUI>().text);
        GameObject.Find("Number").GetComponent<TMPro.TextMeshProUGUI>().text = (number + 1).ToString();
    }

    private Room GenerateDungeon()
    {
        int gridSize = 3 * numberOfRooms;

        rooms = new Room[gridSize, gridSize];

        Vector2 initialRoomCoords = new Vector2((gridSize / 2) - 1, (gridSize / 2) - 1);

        Queue<Room> roomsToCreate = new Queue<Room>();
        roomsToCreate.Enqueue(new Room((int)initialRoomCoords.x, (int)initialRoomCoords.y));
        //Room r = new Room();
        //r.coords = new Vector2((int)initialRoomCoords.x, (int)initialRoomCoords.y);
        //r.neighbors = new Dictionary<string, Room>();
        //roomsToCreate.Enqueue(r);
        
        List<Room> createdRooms = new List<Room>();

        while (roomsToCreate.Count > 0 && createdRooms.Count < numberOfRooms)
        {
            Room currentRoom = roomsToCreate.Dequeue();
            this.rooms[(int)currentRoom.coords.x, (int)currentRoom.coords.y] = currentRoom;
            createdRooms.Add(currentRoom);
            AddNeighbors(currentRoom, roomsToCreate);
        }

        foreach (Room room in createdRooms)
        {
            List<Vector2> neighborCoordinates = room.NeighborCoordinates();
            foreach (Vector2 coordinate in neighborCoordinates)
            {
                Room neighbor = this.rooms[(int)coordinate.x, (int)coordinate.y];
                if (neighbor != null)
                {
                    room.Connect(neighbor);
                }
            }
        }

        return this.rooms[(int)initialRoomCoords.x, (int)initialRoomCoords.y];
    }

    private void AddNeighbors(Room currentRoom, Queue<Room> roomsToCreate)
    {
        List<Vector2> neighborCoordinates = currentRoom.NeighborCoordinates();
        List<Vector2> availableNeighbors = new List<Vector2>();
        foreach (Vector2 coordinate in neighborCoordinates)
        {
            if (this.rooms[(int)coordinate.x, (int)coordinate.y] == null)
            {
                availableNeighbors.Add(coordinate);
            }
        }

        int numberOfNeighbors = (int)Random.Range(1, availableNeighbors.Count);

        for (int neighborIndex = 0; neighborIndex < numberOfNeighbors; neighborIndex++)
        {
            float randomNumber = Random.value;
            float roomFrac = 1f / (float)availableNeighbors.Count;
            Vector2 chosenNeighbor = new Vector2(0, 0);
            foreach (Vector2 coordinate in availableNeighbors)
            {
                if (randomNumber < roomFrac)
                {
                    chosenNeighbor = coordinate;
                    break;
                }
                else
                {
                    roomFrac += 1f / (float)availableNeighbors.Count;
                }
            }
            Room newRoom = new Room(chosenNeighbor);
            //Room newRoom = new Room();
            //newRoom.coords = chosenNeighbor;
            //newRoom.neighbors = new Dictionary<string, Room>();

            newRoom.SetRandomRoom(roomTemplates.Count);

            // check si les enemies doivent spawn
            
            if (UnityEngine.Random.Range(0.0f, 1.0f) < tauxSpawn)
            {
                Vector2 size = roomTemplates[newRoom.IndexRoom].GetComponent<Renderer>().bounds.size;
                newRoom.AddMob(enemyTemplates, minEnemySpawn, maxEnemySpawn, size, roomScenes[newRoom.IndexRoom]);
            }
            
            roomsToCreate.Enqueue(newRoom);
            availableNeighbors.Remove(chosenNeighbor);
        }
    }

    public void MoveToRoom(Room room)
    {
        // encore dans la salle
        if (this.currentRoom.Bonus != null)
        {
            currentRoom.Bonus.gameObject.SetActive(false);
        }

        roomScenes[CurrentRoom().IndexRoom].SetActive(false); // changement de salle
        this.currentRoom = room;
            
        if (this.currentRoom.Bonus != null)
        {
            currentRoom.Bonus.gameObject.SetActive(true);
        }

        roomScenes[CurrentRoom().IndexRoom].SetActive(true);

    }

    public Room CurrentRoom()
    {
        return this.currentRoom;
    }

    public void GenerateDoors()
    {
        Destroy(NDoor);
        Destroy(EDoor);
        Destroy(WDoor);
        Destroy(SDoor);

        foreach (var roomNeighbor in currentRoom.neighbors)
        {
            switch (roomNeighbor.Key)
            {
                case "N":
                    if (roomNeighbor.Value.IsBoss == true)
                    {
                        NDoor = Instantiate(Resources.Load("BossDoor"), new Vector3(0, 4, -5), Quaternion.AngleAxis(0, Vector3.forward)) as GameObject;
                    }
                    else
                    {
                        NDoor = Instantiate(Resources.Load("Door"), new Vector3(0, 4, -5), Quaternion.AngleAxis(0, Vector3.forward)) as GameObject;
                    }
                    
                    NDoor.name = "N";
                    break;
                case "E":
                    if (roomNeighbor.Value.IsBoss == true)
                    {
                        EDoor = Instantiate(Resources.Load("BossDoor"), new Vector3(-9.1f, 0, -5), Quaternion.AngleAxis(90, Vector3.forward)) as GameObject;
                    }
                    else
                    {
                        EDoor = Instantiate(Resources.Load("Door"), new Vector3(-9.1f, 0, -5), Quaternion.AngleAxis(90, Vector3.forward)) as GameObject;
                    }
 
                    EDoor.name = "E";
                    break;
                case "W":
                    if (roomNeighbor.Value.IsBoss == true)
                    {
                        WDoor = Instantiate(Resources.Load("BossDoor"), new Vector3(9.1f, 0, -5), Quaternion.AngleAxis(-90, Vector3.forward)) as GameObject;
                    }
                    else
                    {
                        WDoor = Instantiate(Resources.Load("Door"), new Vector3(9.1f, 0, -5), Quaternion.AngleAxis(-90, Vector3.forward)) as GameObject;
                    }
                        
                    WDoor.name = "W";
                    break;
                case "S":
                    if (roomNeighbor.Value.IsBoss == true)
                    {
                        SDoor = Instantiate(Resources.Load("BossDoor"), new Vector3(0, -4, -5), Quaternion.AngleAxis(180, Vector3.forward)) as GameObject;
                    }
                    else
                    {
                        SDoor = Instantiate(Resources.Load("Door"), new Vector3(0, -4, -5), Quaternion.AngleAxis(180, Vector3.forward)) as GameObject;
                    }
                    
                    SDoor.name = "S";
                    break;
            }
        }
    }

    private void SetRoomBonus()
    {
        BonusManager bm = GameObject.FindGameObjectWithTag("BonusManager").GetComponent<BonusManager>();
        GameObject bonus = bm.GetRandomBonus();

        int randomRowNumber = Random.Range(0, this.rooms.GetLength(1));
        int randomColNumber = Random.Range(0, this.rooms.GetLength(0));

        while (this.rooms[randomColNumber, randomRowNumber] == null)
        {
            randomRowNumber = Random.Range(0, this.rooms.GetLength(1));
            randomColNumber = Random.Range(0, this.rooms.GetLength(0));
        }

        this.rooms[randomColNumber, randomRowNumber].Bonus = bm.SpawnSelectedBonus(Vector2.zero, bonus);

        GameObject b = GameObject.FindGameObjectWithTag("Bonus");
        b.gameObject.SetActive(false);
    }

    private void SetBossRoom()
    {
        float rowNumber = 0;
        float colNumber = 0;


        foreach (var room in this.rooms)
        {
            if (room != null && room != this.currentRoom && room.neighbors.Count == 1)
            {
                rowNumber = room.coords.x;
                colNumber = room.coords.y;
                break;
            }
        }

        if (rowNumber == 0 && colNumber == 0)
        {
            Init();
        }
        else
        {
            this.rooms[(int)rowNumber, (int)colNumber].IsBoss = true;
        }

        
    }

    private void InitRoomsScene()
    {
        for (int i = 0; i < roomTemplates.Count; i++)
        {
            roomScenes.Add(Instantiate(roomTemplates[i], Vector2.zero, Quaternion.identity));
            roomScenes[i].SetActive(false);
        }
    }

    private void PrintGrid()
    {
        string str = "";
        for (int rowIndex = 0; rowIndex < this.rooms.GetLength(0); rowIndex++)
        {
            string row = "";
            for (int columnIndex = 0; columnIndex < this.rooms.GetLength(1); columnIndex++)
            {
                if (this.rooms[columnIndex, rowIndex] == null)
                {
                    row += "X";
                }
                else
                {
                    row += "R";
                }
            }
            str += row + "\n";
        }
        Debug.Log(str);
    }

}
