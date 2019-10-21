using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class Door : MonoBehaviour
    {
        private string direction;
        private SoundEffect se;
        private Animator anim;
        private bool roomStatus = true;
        private bool dungeonStatus = true;
        private GameObject dungeon;
        private DungeonGenerator dungeonGenerator;
        private Room room; 

        private void Start()
        {
            se = GameObject.Find("DoorSound").GetComponent<SoundEffect>();
            anim = GetComponent<Animator>();
            dungeon = GameObject.FindGameObjectWithTag("Dungeon");

            if (dungeon == null && SceneManager.GetActiveScene().name != "Stage")
            {
                dungeon = GameObject.Find("Dungeon");
            }

            dungeonGenerator = dungeon.GetComponent<DungeonGenerator>();
            room = dungeonGenerator.CurrentRoom();
        }

        private void Update()
        {
            foreach (var enemy in room.Enemies)
            {
                if (enemy.activeSelf)
                {
                    roomStatus = false;
                    break;
                }
                else
                {
                    roomStatus = true;
                }
            }

            dungeonStatus = true;

            foreach (var r in dungeonGenerator.Rooms)
            {
                if(r != null)
                {
                    foreach (var enemy in r.Enemies)
                    {
                        if(r.IsBoss == true)
                        {
                            Destroy(enemy);
                        }
                        else
                        {
                            if (enemy.GetComponent<IAEnemy>().current_hp > 0 && r.IsBoss != true)
                            {
                                dungeonStatus = false;
                                break;
                            }
                        }
                    }
                }
            }
            StartCoroutine(OpeningDoor());
        }

        IEnumerator OpeningDoor()
        {
            if (roomStatus)
            {
                anim.SetInteger("DoorState", 1);

                yield return new WaitForSeconds(1);

                anim.SetInteger("DoorState", 2);

            }

            if (dungeonStatus)
            {
                anim.SetInteger("BossDoorState", 1);

                yield return new WaitForSeconds(1);

                anim.SetInteger("BossDoorState", 2);
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                if (roomStatus == true)
                {
                    foreach (var neighbor in room.neighbors)
                    {
                        if (neighbor.Key == this.name)
                        {
                            direction = neighbor.Key;
                        }
                    }

                    if(room.Neighbor(this.direction).IsBoss == true)
                    {
                        if (dungeonStatus)
                        {
                            dungeonGenerator.CurrentRoom().RemoveMobs();
                            dungeonGenerator.GenerateDoors();

                            DontDestroyOnLoad(this.gameObject);
                            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Player"));
                            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("SoundManager"));

                            GameObject.Find("InGameTheme").GetComponent<CaveSound>().Source.Stop();
                            DontDestroyOnLoad(GameObject.Find("InGameTheme"));

                            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Level"));

                            SceneManager.LoadScene("Boss");
                        }
                    }
                    else
                    {
                        dungeonGenerator.CurrentRoom().RemoveMobs();
                        dungeonGenerator.MoveToRoom(room.Neighbor(this.direction));
                        switch (direction)
                        {
                            case "N":
                                Destroy(dungeonGenerator.NDoor);
                                break;
                            case "E":
                                Destroy(dungeonGenerator.EDoor);
                                break;
                            case "W":
                                Destroy(dungeonGenerator.WDoor);
                                break;
                            case "S":
                                Destroy(dungeonGenerator.SDoor);
                                break;
                        }

                        dungeonGenerator.GenerateDoors();

                        dungeonGenerator.CurrentRoom().SpawnMob();
                        GameObject player = GameObject.FindGameObjectWithTag("Player");

                        switch (this.direction)
                        {
                            case "N":
                                player.transform.position = new Vector3(0, -2, -5);
                                break;
                            case "E":
                                player.transform.position = new Vector3(7, 0, -5);
                                break;
                            case "W":
                                player.transform.position = new Vector3(-7, 0, -5);
                                break;
                            case "S":
                                player.transform.position = new Vector3(0, 2, -5);
                                break;
                        }
                    }

                    // play sound
                    se.PlaySound();
                }
            }
        }
    }
}
