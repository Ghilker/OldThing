using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class roomStats : MonoBehaviour
{
    public int width;
    public int height;
    public Vector2 roomCoordinates;
    public List<GameObject> connectedRooms;
    public List<GameObject> doors;
    [EnumFlagsAttribute] public direction connectedDirs;
    public int roomDepth = 0;
    public bool isActive = true;
    public List<GameObject> monsters = new List<GameObject>();
    public List<GameObject> monsterSpawners = new List<GameObject>();
    public List<Vector3> internalGridPositions = new List<Vector3>();
    bool canSpawn = true;
    public bool isSpecial = false;

    public void Connect(GameObject otherRoom, direction dir)
    {
        connectedRooms.Add(otherRoom);
    }

    private void Update()
    {
        if (!isActive) { return; }
        bool shouldDeactivate = true;
        if (canSpawn)
        {
            SpawnMonsters();
            canSpawn = false;
        }
        foreach (GameObject monster in monsters)
        {
            if (monster.GetComponent<MonsterStats>().isAlive)
            {
                shouldDeactivate = false;
                break;
            }
        }
        if (!shouldDeactivate)
        {
            return;
        }
        isActive = false;
        foreach (GameObject door in doors)
        {
            door.GetComponent<DoorStats>().DoorDisable();
        }
    }

    void SpawnMonsters()
    {
        foreach (GameObject monsterSpawner in monsterSpawners)
        {
            if (monsterSpawner.GetComponent<MonsterSpawn>().canSpawn)
            {
                monsterSpawner.GetComponent<MonsterSpawn>().SpawnMonsters();
            }
        }
        foreach (GameObject door in doors)
        {
            door.GetComponent<DoorStats>().DoorEnable();
        }
    }

}
