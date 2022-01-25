using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Helper;

public class roomStats : MonoBehaviour
{
    public int width;
    public int height;
    public direction connectedDirs;
    public Vector3 roomCoordinates;
    public List<Vector3> internalGrid = new List<Vector3>();
    public List<GameObject> doors;
    public int roomDepth = 0;
    public bool isActive = false;
    public RoomGenerator ourGen;
    public List<GameObject> monsters = new List<GameObject>();
    public List<GameObject> monsterSpawners;
    public List<GameObject> obstacleSpawners;
    public bool canSpawn = true;
    public bool isSpecial = false;
    public bool isBossRoom = false;
    public bool canProcess = true;
    public NavMeshSurface navMesh;

    public bool builtNavMesh = false;
    private void Update()
    {
        if (!canProcess)
        {
            foreach (GameObject door in doors)
            {
                if (door == null)
                {
                    continue;
                }
                door.GetComponent<DoorStats>().DoorDisable();
            }
            return;
        }
        if (builtNavMesh == false)
        {
            if (!navMesh)
            {
                navMesh = GetComponent<NavMeshSurface>();
            }
            navMesh.BuildNavMesh();
            builtNavMesh = true;
        }
        if (!isActive) { return; }

        bool shouldDeactivate = true;
        if (canSpawn)
        {
            SpawnObstacles();
            navMesh.BuildNavMesh();
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
                monsterSpawner.GetComponent<MonsterSpawn>().room = GetComponent<roomStats>();
                monsterSpawner.GetComponent<MonsterSpawn>().SpawnMonsters();
            }
        }
        foreach (GameObject door in doors)
        {
            door.GetComponent<DoorStats>().DoorEnable();
        }
    }
    void SpawnObstacles()
    {
        foreach (GameObject obstacleSpawner in obstacleSpawners)
        {
            if (obstacleSpawner != null && obstacleSpawner.GetComponent<obstacleSpawner>().canSpawn)
            {
                obstacleSpawner.GetComponent<obstacleSpawner>().SpawnObstacle();
            }
        }
    }

}
