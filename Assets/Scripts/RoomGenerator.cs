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
        instancedRoomStats.roomCoordinates = roomPosition;
        GameObject wallHolder = new GameObject("wallHolder");
        GameObject floorHolder = new GameObject("floorHolder");
        wallHolder.transform.SetParent(instancedRoom.transform);
        floorHolder.transform.SetParent(instancedRoom.transform);
        int obstacleNumbers = 0;

        int middleX = Mathf.RoundToInt(width / 2f);
        int middleY = Mathf.RoundToInt(height / 2f);
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
                    direction dir = CheckDirection(x, y, middleX, middleY);

                    Vector3 connectorPosition = DirectionalMovement.GetVectorOffsetInDir(dir, new Vector3(x, y, 0f));

                    GameObject connector = Instantiate(outsideConnector, connectorPosition, Quaternion.identity);
                    connector.name = "connector_" + dir;
                    connector.tag = "Connector";
                    connector.transform.SetParent(instancedRoom.transform);
                    connector.GetComponent<ConnectorStat>().dir = dir;
                    GameObject placedDoor = Instantiate(door, new Vector3(x, y, 0f), Quaternion.identity);
                    placedDoor.name = "door_" + dir;
                    placedDoor.transform.SetParent(instancedRoom.transform);
                    placedDoor.GetComponent<DoorStats>().dir = dir;
                    instancedRoomStats.doors.Add(placedDoor);
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
                Vector3 monsterPosition = RandomPosition();
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

    direction CheckDirection(int x, int y, int middleX, int middleY)
    {
        direction dir = direction.NORTH;
        if (y > middleX)
            dir = direction.NORTH;
        else if (y < middleX)
            dir = direction.SOUTH;
        else if (x > middleY)
            dir = direction.EAST;
        else if (x < middleY)
            dir = direction.WEST;
        return dir;
    }

    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
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
    LOW = 5,
    MID = 10,
    HIGH = 15
}