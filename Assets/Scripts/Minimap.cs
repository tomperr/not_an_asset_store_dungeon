using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets;

public class Minimap : MonoBehaviour
{

    [SerializeField] List<Sprite> Sprites;
    private Room[,] rooms;
    private GameObject[,] minimaps;
    private GameObject dungeon;


    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init()
    {
        dungeon = GameObject.FindGameObjectWithTag("Dungeon");
        if(dungeon == null && SceneManager.GetActiveScene().name != "Stage")
        {
            dungeon = GameObject.Find("Dungeon");
        }

        rooms = dungeon.GetComponent<DungeonGenerator>().Rooms;
        
        int gridSize = dungeon.GetComponent<DungeonGenerator>().NumberOfRooms * 3;
        minimaps = new GameObject[gridSize, gridSize];

        GenerateMinimap();
        ChangeMinimapCurrent(dungeon.GetComponent<DungeonGenerator>().CurrentRoom());
        HideAllRooms();
    }

    // Update is called once per frame
    void Update()
    {
        if (dungeon != null)
        {
            CurrentToExplored(FindRoom(dungeon.GetComponent<DungeonGenerator>().CurrentRoom()));
            ChangeMinimapCurrent(dungeon.GetComponent<DungeonGenerator>().CurrentRoom());
        }
    }

    public void GenerateMinimap()
    {

        Reverse2DArrayVert<Room>(rooms);
        
        foreach (var room in rooms)
        {
            if (room != null)
            {
                GameObject minimapRoom = Instantiate(Resources.Load("MinimapExplored")) as GameObject;
                minimapRoom.transform.SetParent(transform.GetChild(0));
                minimapRoom.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);

                Vector2 position = new Vector2(-room.coords.x * 10, -room.coords.y * 10);
                minimapRoom.transform.position = position;
                minimaps[(int)room.coords.x, (int)room.coords.y] = minimapRoom;
            }
        }

    }

    public void ChangeMinimapCurrent(Room room)
    {
        Room newRoom = FindRoom(room);
        ShowRoomAndNeighbours(newRoom);
        CenterMinimap(newRoom);
    }

    private void CurrentToExplored(Room room)
    {
        foreach(var roomx in minimaps)
        {
            if (roomx != null && roomx.GetComponent<Image>().sprite == Sprites[2])
                roomx.GetComponent<Image>().sprite = Sprites[0];
        }
    }

    public void CenterMinimap(Room room)
    {
        Vector2 roomPos = minimaps[(int)room.coords.x, (int)room.coords.y].transform.position;
        Vector2 parentPos = transform.GetChild(0).transform.position;
        Vector2 minimapPos = transform.position;

        Vector2 offset = minimapPos - roomPos;
        transform.GetChild(0).transform.position = parentPos + offset;
    }

    public void HideAllRooms()
    {
        foreach (var room in minimaps)
        {
            if (room != null)
            {
                room.gameObject.SetActive(false);
                room.GetComponent<Image>().sprite = Sprites[1];
            }
            
        }
    }

    public void ShowRoom(Room room)
    {
        minimaps[(int)room.coords.x, (int)room.coords.y].gameObject.SetActive(true);
    }

    public void ShowRoom(Vector2 position)
    {
        minimaps[(int)position.x, (int)position.y].gameObject.SetActive(true);
    }

    public void ShowRoomAndNeighbours(Room room)
    {
        ShowRoom(room);
        foreach (var item in room.neighbors)
        {
            ShowRoom(item.Value);
            minimaps[(int)room.coords.x,(int)room.coords.y].GetComponent<Image>().sprite = Sprites[2];
        }
    }

    public void Reverse2DArray<T>(T[,] array)
    {
        for (int rowIndex = 0;
             rowIndex <= (array.GetUpperBound(0)); rowIndex++)
        {
            for (int colIndex = 0;
                 colIndex <= (array.GetUpperBound(1) / 2); colIndex++)
            {
                T tempHolder = array[rowIndex, colIndex];
                array[rowIndex, colIndex] = array[rowIndex, array.GetUpperBound(1) - colIndex];
                array[rowIndex, array.GetUpperBound(1) - colIndex] = tempHolder;
            }
        }
    }

    public Room FindRoom(Room room)
    {
        foreach(var roomx in rooms)
        {
            if (roomx != null && roomx.Index == room.Index)
            {
                return roomx;
            }
        }
        return null;
    }

    public void Reverse2DArrayVert<T>(T[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        for (int i = 0; i <= cols - 1; i++)
        {
            int j = 0;
            int k = rows - 1;
            while (j < k)
            {
                T temp = array[j, i];
                array[j, i] = array[k, i];
                array[k, i] = temp;
                j++;
                k--;
            }
        }
    }
}
