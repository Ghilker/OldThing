using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class roomStats : MonoBehaviour
{
    public int width;
    public int height;
    public Vector2 roomCoordinates;
    public List<Vector2> internalGrid = new List<Vector2>();
    public List<GameObject> doors;
    public int roomDepth = 0;
    public bool isActive = true;
    public List<GameObject> monsters = new List<GameObject>();
    public List<GameObject> monsterSpawners = new List<GameObject>();
    [SerializeField]
    bool canSpawn = true;
    public bool isSpecial = false;

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
            if (monster == null)
            {
                continue;
            }
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
            if (door == null)
            {
                continue;
            }
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
