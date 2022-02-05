using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Helper;
using static Helper.DirectionalMovement;

public class BoardMaker : MonoBehaviour
{
    [SerializeField]
    //All the rooms made in the current level
    List<GameObject> generatedRooms;

    [SerializeField]
    //List of vector3 that indicates the generated rooms positions
    List<Vector3> roomGridPositions = new List<Vector3>();

    //List of the random empty spots in the grid
    List<Vector2> randomPositions = new List<Vector2>();

    public GameObject intialRoom;

    public List<GameObject> bossRooms;
    [SerializeField]
    //GameObject bossRoom;
    //bool hasBoss = false;

    public GameObject player;
    public GameObject closingWallObj;

    //Main holder for all game objects
    GameObject Board;
    //Max dept that the rooms can reach
    [SerializeField]
    int maxRoomDepth = 4;

    //Reference to the camera
    Camera mainCamera;

    List<GameObject> specialRooms = new List<GameObject>();

    public GameObject monsterSpawner;

    public RoomGenerator gen;

    public bool active = true;

    public void BoardInit(int dungeonDepth)
    {
        if (!active)
        {
            return;
        }
        mainCamera = Camera.main;
        maxRoomDepth = dungeonDepth;
        GenerateBoard();
    }

    void GenerateBoard()
    {
        //Create board holder
        Board = new GameObject("BoardHolder");
        CreateRoom();
        SealDungeon();
        PopulateDungeon();
        HideDungeon();
    }

    void CreateRoom()
    {
        generatedRooms = new List<GameObject>();
        GameObject firstRoom = Instantiate(intialRoom, Vector3.zero, Quaternion.identity);
        firstRoom.transform.SetParent(Board.transform);
        generatedRooms.Add(firstRoom);
        roomGridPositions.Add(firstRoom.transform.position);
        roomStats firstRoomStats = firstRoom.GetComponent<roomStats>();
        firstRoomStats.isSpecial = true;
        firstRoomStats.canProcess = false;
        firstRoomStats.roomCoordinates = Vector3.zero;
        firstRoomStats.ourGen = gen;
        int middleX = firstRoomStats.width / 2;
        int middleZ = firstRoomStats.height / 2;
        Instantiate(player, new Vector3(middleX, 0f, middleZ), Quaternion.identity);
        Vector3 cameraPosition = new Vector3((firstRoomStats.width + 1) / 2, 10, (firstRoomStats.height + 1) / 2);
        Camera.main.transform.position = firstRoom.transform.position + cameraPosition;
        if (maxRoomDepth < 1)
        {
            return;
        }
        BranchOut(firstRoom);
    }

    void BranchOut(GameObject oldRoom)
    {
        roomStats oldRoomStats = oldRoom.GetComponent<roomStats>();
        List<GameObject> oldDoors = SearchChildren.SearchAllChildTag(oldRoom, "Door");
        List<direction> directions = new List<direction>() { direction.NORTH, direction.SOUTH, direction.EAST, direction.WEST };
        oldRoomStats.doors = oldDoors;
        RandomHelper.ShuffleList(directions);
        if (RandomHelper.prob(10))
        {
            directions.Remove(directions[Random.Range(0, directions.Count)]);
            if (RandomHelper.prob(10))
            {
                directions.Remove(directions[Random.Range(0, directions.Count)]);
            }
        }
        foreach (direction dir in directions)
        {
            if (oldRoomStats.connectedDirs.HasFlag(dir))
            {
                continue;
            }
            Vector3 currentPosition = oldRoom.transform.position;
            direction originalDir = dir;
            int currentDepth = oldRoomStats.roomDepth;
            Vector3 newRoomPosition = DirectionalMovement.GetVectorOffsetInDir(dir, oldRoom.transform.position, oldRoomStats.width + 1, 0, oldRoomStats.height + 1);
            Vector3 newRoomPositionGrid = DirectionalMovement.MoveTo(dir, oldRoomStats.roomCoordinates);
            if (RandomHelper.prob(20) && currentDepth > 2)
            {
                randomPositions.Add(newRoomPositionGrid);
                continue;
            }

            if (roomGridPositions.Contains(newRoomPositionGrid) || randomPositions.Contains(newRoomPositionGrid))
            {
                continue;
            }
            roomGridPositions.Add(newRoomPositionGrid);

            GameObject newRoom = gen.GenerateRoom(newRoomPosition);
            generatedRooms.Add(newRoom);
            newRoom.transform.SetParent(Board.transform);

            roomStats newRoomStats = newRoom.GetComponent<roomStats>();
            newRoomStats.roomCoordinates = newRoomPositionGrid;
            newRoomStats.roomDepth = currentDepth + 1;
            oldRoomStats.connectedDirs |= originalDir;
            newRoomStats.connectedDirs |= DirectionalMovement.ReverseDirection(originalDir);
            newRoomStats.ourGen = gen;

            newRoomStats.doors = SearchChildren.SearchAllChildTag(newRoom, "Door");
            GameObject ourDoor = null;
            GameObject otherDoor = null;
            foreach (GameObject door in oldRoomStats.doors)
            {
                DoorStats doorStat = door.GetComponent<DoorStats>();
                if (doorStat.dir != originalDir)
                {
                    continue;
                }
                doorStat.connectingRoom = newRoom;
                ourDoor = door;
            }
            foreach (GameObject door in newRoomStats.doors)
            {
                DoorStats doorStat = door.GetComponent<DoorStats>();
                if (doorStat.dir != DirectionalMovement.ReverseDirection(originalDir))
                {
                    continue;
                }
                doorStat.connectingRoom = oldRoom;
                otherDoor = door;
            }

            ourDoor.GetComponent<DoorStats>().otherDoorObj = otherDoor;
            otherDoor.GetComponent<DoorStats>().otherDoorObj = ourDoor;

            if (newRoomStats.roomDepth < maxRoomDepth)
            {
                BranchOut(newRoom);
            }
        }

    }

    void SealDungeon()
    {
        foreach (GameObject currentRoom in generatedRooms)
        {
            List<GameObject> currentDoors = SearchChildren.SearchAllChildTag(currentRoom, "Door");
            foreach (GameObject currentDoor in currentDoors)
            {
                if (currentDoor.GetComponent<DoorStats>().connectingRoom == null)
                {
                    GameObject closingWall = Instantiate(closingWallObj, currentDoor.transform.position, Quaternion.identity);
                    closingWall.transform.SetParent(currentRoom.transform.GetChild(0));
                    currentRoom.GetComponent<roomStats>().doors.Remove(currentDoor);
                    Destroy(currentDoor);
                }
            }
        }
    }

    void PopulateDungeon()
    {
        foreach (GameObject currentRoom in generatedRooms)
        {
            if (currentRoom == generatedRooms[0])
            {
                continue;
            }
            roomStats currentRoomStats = currentRoom.GetComponent<roomStats>();
            foreach (GameObject door in currentRoomStats.doors)
            {
                for (int i = -3; i < 3; i++)
                {
                    for (int j = -3; j < 3; j++)
                    {
                        Vector3 positionToRemove = door.transform.localPosition + new Vector3(i, 0f, j);
                        currentRoomStats.internalGrid.Remove(positionToRemove);
                    }
                }
            }
            currentRoomStats.ourGen.ObstacleCreate(currentRoom);
            currentRoomStats.ourGen.MonsterCreate(currentRoom);
        }
    }

    void HideDungeon()
    {
        foreach (GameObject currentRoom in generatedRooms)
        {
            if (currentRoom == generatedRooms[0])
            {
                continue;
            }
            currentRoom.SetActive(false);
        }
    }
}
