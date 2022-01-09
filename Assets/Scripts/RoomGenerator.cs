using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

[CreateAssetMenu(menuName = "Generator/RoomGen")]
public class RoomGenerator : ScriptableObject
{
    public GameObject outsideConnector;
    public GameObject roomHolder;
    public GameObject monsterSpawner;

    public GameObject[] floorArray;
    public GameObject[] wallArray;

    public GameObject[] obstacleArray;

    public GameObject door;

    public int width = 8;
    public int height = 8;

    public int monsterSpawnerAmount = 4;
    public ObstacleRandomness obstacleRandomness = ObstacleRandomness.LOW;

    private List<Vector3> gridPositions = new List<Vector3>();


    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        //Clear our list gridPositions.
        gridPositions.Clear();

        //Loop through x axis (columns).
        for (int x = 1; x < width - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < height - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    public GameObject GenerateRoom(Vector2 roomPosition, bool firstRoom = false, bool specialRoom = false)
    {
        InitialiseList();
        GameObject instancedRoom = Instantiate(roomHolder, Vector3.zero, Quaternion.identity);
        instancedRoom.name = "room_" + width + "x" + height;
        instancedRoom.tag = "RoomHolder";
        roomStats instancedRoomStats = instancedRoom.GetComponent<roomStats>();
        instancedRoomStats.width = width;
        instancedRoomStats.height = height;
        GameObject wallHolder = new GameObject("wallHolder");
        GameObject floorHolder = new GameObject("floorHolder");
        wallHolder.transform.SetParent(instancedRoom.transform);
        floorHolder.transform.SetParent(instancedRoom.transform);
        int obstacleNumbers = 0;

        int middleX = Mathf.RoundToInt(width / 2f);
        int middleY = Mathf.RoundToInt(height / 2f);
        Vector3 middlePoint = new Vector3(middleX, middleY, 0f);
        bool isWall = false;
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                isWall = false;
                GameObject toInstantiate = floorArray[Random.Range(0, floorArray.Length)];
                if ((x == 0 || x == width || y == 0 || y == height) && (x != middleX && y != middleY))
                {
                    isWall = true;
                    toInstantiate = wallArray[Random.Range(0, wallArray.Length)];
                }
                if ((x == 0 || x == width || y == 0 || y == height) && (x == middleX || y == middleY))
                {
                    Vector3 currentXY = new Vector3(x, y, 0f);

                    direction dir = DirectionalMovement.CheckVectorialDirection(middlePoint, currentXY);

                    Vector3 connectorPosition = DirectionalMovement.GetVectorOffsetInDir(dir, new Vector3(x, y, 0f));

                    CreateConnector(dir, connectorPosition, instancedRoom.transform);
                    CreateDoor(dir, new Vector3(x, y, 0f), instancedRoom.transform, instancedRoomStats);

                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                if (isWall == true)
                {
                    instance.transform.SetParent(wallHolder.transform);
                }
                else
                {
                    instance.transform.SetParent(floorHolder.transform);
                    if (RandomHelper.prob((int)obstacleRandomness) && (x != middleX && y != middleY) && !firstRoom && !specialRoom)
                    {
                        GameObject obstacleToInstantiate = obstacleArray[Random.Range(0, obstacleArray.Length)];
                        GameObject obstacleInstance = Instantiate(obstacleToInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                        obstacleInstance.transform.SetParent(instance.transform);
                        obstacleNumbers++;
                    }
                }
            }
        }

        if (!firstRoom && !specialRoom)
        {
            GameObject monsterHolder = new GameObject("monsterHolder");
            monsterHolder.transform.SetParent(instancedRoom.transform);

            for (int i = 0; i < monsterSpawnerAmount; i++)
            {
                Vector3 monsterPosition = RandomHelper.RandomPosition(gridPositions);
                RaycastHit2D hit = Physics2D.Raycast(monsterPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.tag == "Obstacle")
                {
                    i--;
                    continue;
                }
                GameObject monsterSpawn = Instantiate(monsterSpawner, monsterPosition, Quaternion.identity);
                monsterSpawn.transform.SetParent(monsterHolder.transform);
                monsterSpawn.GetComponent<MonsterSpawn>().room = instancedRoomStats;
                monsterSpawn.GetComponent<MonsterSpawn>().canSpawn = true;
                instancedRoomStats.monsterSpawners.Add(monsterSpawn);

            }
        }
        instancedRoom.name = instancedRoom.name + "_" + obstacleNumbers + "-obstacles";
        instancedRoom.transform.localPosition = Vector3.zero;
        instancedRoom.transform.position = roomPosition;
        return instancedRoom;
    }

    void CreateConnector(direction dir, Vector3 position, Transform parent)
    {
        GameObject connector = Instantiate(outsideConnector, position, Quaternion.identity);
        connector.name = "connector_" + dir;
        connector.tag = "Connector";
        connector.transform.SetParent(parent);
        connector.GetComponent<ConnectorStat>().dir = dir;
    }

    void CreateDoor(direction dir, Vector3 position, Transform parent, roomStats parentStats)
    {
        GameObject placedDoor = Instantiate(door, position, Quaternion.identity);
        placedDoor.name = "door_" + dir;
        placedDoor.transform.SetParent(parent);
        placedDoor.GetComponent<DoorStats>().dir = dir;
        parentStats.doors.Add(placedDoor);
    }

}

[System.Flags]
public enum direction
{
    NORTH = (1 << 0),
    SOUTH = (1 << 1),
    EAST = (1 << 2),
    WEST = (1 << 3)
}

public enum ObstacleRandomness
{
    LOW = 1,
    MID = 3,
    HIGH = 5
}