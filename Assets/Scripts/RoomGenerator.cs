using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

[CreateAssetMenu(menuName = "Generator/RoomGen")]
public class RoomGenerator : ScriptableObject
{
    public GameObject outsideConnector;
    public GameObject roomHolder;

    public GameObject[] floorArray;
    public GameObject[] wallArray;

    public GameObject[] obstacleArray;
    public GameObject monsterSpawner;

    public GameObject door;

    public int width = 8;
    public int height = 8;

    public int minObstacleAmount = 5;
    public int maxObstacleAmount = 8;
    public int minMonsterAmount = 4;
    public int maxMonsterAmounts = 8;

    GameObject instancedRoom;
    roomStats instancedRoomStats;

    List<Vector3> InitialiseList()
    {
        List<Vector3> gridPositions = new List<Vector3>();
        for (int x = 1; x < width; x++)
        {
            for (int z = 1; z < height; z++)
            {
                gridPositions.Add(new Vector3(x, 0f, z));
            }
        }
        return gridPositions;
    }

    public GameObject GenerateRoom(Vector3 roomPosition)
    {
        instancedRoom = Instantiate(roomHolder, Vector3.zero, Quaternion.identity);
        instancedRoom.name = "room_" + width + "x" + height;
        instancedRoom.tag = "RoomHolder";
        instancedRoomStats = instancedRoom.GetComponent<roomStats>();
        instancedRoomStats.width = width;
        instancedRoomStats.height = height;
        instancedRoomStats.internalGrid = InitialiseList();
        GameObject wallHolder = new GameObject("wallHolder");
        GameObject floorHolder = new GameObject("floorHolder");
        GameObject doorHolder = new GameObject("doorHolder");
        wallHolder.transform.SetParent(instancedRoom.transform);
        floorHolder.transform.SetParent(instancedRoom.transform);
        doorHolder.transform.SetParent(instancedRoom.transform);

        int middleX = Mathf.RoundToInt(width / 2f);
        int middleZ = Mathf.RoundToInt(height / 2f);
        Vector3 middlePoint = new Vector3(middleX, 0f, middleZ);
        bool isWall = false;
        for (int x = 0; x <= width; x++)
        {
            for (int z = 0; z <= height; z++)
            {
                isWall = false;
                Vector3 position = new Vector3(x, 0, z);
                Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
                GameObject toInstantiate = floorArray[Random.Range(0, floorArray.Length)];
                if ((x == 0 || x == width || z == 0 || z == height) && (x != middleX && z != middleZ))
                {
                    isWall = true;
                    toInstantiate = wallArray[Random.Range(0, wallArray.Length)];
                    position = new Vector3(x, 0f, z);
                    rotation = Quaternion.identity;
                }
                if ((x == 0 || x == width || z == 0 || z == height) && (x == middleX || z == middleZ))
                {
                    Vector3 currentXY = new Vector3(x, 0f, z);

                    direction dir = DirectionalMovement.CheckVectorialDirection(middlePoint, currentXY);

                    Vector3 connectorPosition = DirectionalMovement.GetVectorOffsetInDir(dir, new Vector3(x, 0f, z));

                    CreateDoor(dir, new Vector3(x, 0f, z), doorHolder.transform, instancedRoomStats);

                }
                GameObject instance = Instantiate(toInstantiate, position, rotation);
                if (isWall == true)
                {
                    instance.transform.SetParent(wallHolder.transform);
                }
                else
                {
                    instance.transform.SetParent(floorHolder.transform);
                }
            }
        }

        floorHolder.transform.position = new Vector3(0f, -.5f, 0f);
        instancedRoom.transform.localPosition = Vector3.zero;
        instancedRoom.transform.position = roomPosition;
        return instancedRoom;
    }

    void CreateDoor(direction dir, Vector3 position, Transform parent, roomStats parentStats)
    {
        GameObject placedDoor = Instantiate(door, position, Quaternion.identity);
        placedDoor.name = "door_" + dir;
        placedDoor.transform.SetParent(parent);
        placedDoor.GetComponent<DoorStats>().dir = dir;
        parentStats.doors.Add(placedDoor);
    }

    public void ObstacleCreate(GameObject selectedRoom)
    {
        roomStats selectedRoomStats = selectedRoom.GetComponent<roomStats>();
        GameObject obstacleHolder = new GameObject("obstacleHolder");
        obstacleHolder.transform.SetParent(selectedRoom.transform);
        for (int i = 0; i < Random.Range(minObstacleAmount, maxObstacleAmount); i++)
        {
            Vector3 position = RandomHelper.RandomPosition(selectedRoomStats.internalGrid, true) + selectedRoom.transform.position;
            GameObject obstacleInstance = Instantiate(obstacleArray[Random.Range(0, obstacleArray.Length)], position, Quaternion.identity);
            obstacleInstance.transform.SetParent(obstacleHolder.transform);
            selectedRoomStats.obstacleSpawners.Add(obstacleInstance);
        }
    }

    public void MonsterCreate(GameObject selectedRoom)
    {
        roomStats selectedRoomStats = selectedRoom.GetComponent<roomStats>();
        GameObject monsterHolder = new GameObject("monsterHolder");
        monsterHolder.transform.SetParent(selectedRoom.transform);
        for (int i = 0; i < Random.Range(minMonsterAmount, maxMonsterAmounts); i++)
        {
            Vector3 position = RandomHelper.RandomPosition(selectedRoomStats.internalGrid, true) + selectedRoom.transform.position;
            GameObject monsterInstance = Instantiate(monsterSpawner, position, Quaternion.identity);
            monsterInstance.transform.SetParent(monsterHolder.transform);
            selectedRoomStats.monsterSpawners.Add(monsterInstance);
        }
    }

}
