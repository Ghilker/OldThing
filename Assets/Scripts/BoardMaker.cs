using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class BoardMaker : MonoBehaviour
{
    [SerializeField]
    //All the rooms made in the current level
    List<GameObject> generatedRooms;

    [SerializeField]
    //List of vector3 that indicates the generated rooms positions
    List<Vector3> roomPositions = new List<Vector3>();

    //List of the random empty spots in the grid
    List<Vector3> randomPositions = new List<Vector3>();

    //Current room generator
    public RoomGenerator gen;

    //Main holder for all game objects
    GameObject Board;
    //Max dept that the rooms can reach
    public int maxRoomDepth = 4;
    //Size of the rooms
    public RoomSize roomSize;

    //Reference to the camera
    Camera mainCamera;

    private void Start()
    {
        gen.roomSize = (int)roomSize;
        mainCamera = Camera.main;
        GenerateBoard();
    }

    void GenerateBoard()
    {
        //Create board holder
        Board = new GameObject("BoardHolder");
        CreateRoom();
        SealDungeon();
        HideDungeon();
    }

    void CreateRoom()
    {
        //Creation of the first room
        generatedRooms = new List<GameObject>();
        GameObject firstRoom = gen.GenerateRoom(Vector2.zero, true);
        roomPositions.Add(firstRoom.transform.position);
        firstRoom.transform.SetParent(Board.transform);
        generatedRooms.Add(firstRoom);
        //Adjusting position of the camera to the center of the first room
        Vector3 cameraPosition = new Vector3((firstRoom.GetComponent<roomStats>().width + 1) / 2, (firstRoom.GetComponent<roomStats>().width + 1) / 2, -10f);
        mainCamera.transform.position = cameraPosition + new Vector3(0.5f, 0.5f, 0f);
        BranchOut(firstRoom);
    }

    void BranchOut(GameObject startingRoom)
    {
        List<GameObject> connectors = SearchChildren.SearchForTag(startingRoom, "Connector");
        List<GameObject> doors = SearchChildren.SearchForTag(startingRoom, "Door");
        roomStats starterRoom = startingRoom.GetComponent<roomStats>();
        int randomToRemove = Random.Range(0, connectors.Count);
        connectors.Remove(connectors[randomToRemove]);
        if (RandomHelper.prob(25))
        {
            randomToRemove = Random.Range(0, connectors.Count);
            connectors.Remove(connectors[randomToRemove]);
            if (RandomHelper.prob(25) && starterRoom.roomDepth > 2)
            {
                randomToRemove = Random.Range(0, connectors.Count);
                connectors.Remove(connectors[randomToRemove]);
            }
        }

        foreach (GameObject connector in connectors)
        {

            if (connector.GetComponent<ConnectorStat>().connected)
            {
                continue;
            }

            Vector3 currentPosition = connector.transform.position;
            direction originalDir = connector.GetComponent<ConnectorStat>().dir;
            int currentDepth = starterRoom.roomDepth;
            direction currentDir = originalDir;
            int xOffset = Mathf.RoundToInt(starterRoom.width / 2);
            int yOffset = Mathf.RoundToInt(starterRoom.height / 2);
            int offset = 1;
            if (originalDir.HasFlag(direction.NORTH) || originalDir.HasFlag(direction.SOUTH))
            {
                offset = yOffset;
            }
            else if (originalDir.HasFlag(direction.EAST) || originalDir.HasFlag(direction.WEST))
            {
                offset = xOffset;
            }

            Vector3 newRoomPosition = DirectionalMovement.MoveVectorToOffset(currentPosition, originalDir, offset);
            if (RandomHelper.prob(25) && starterRoom.roomDepth > 2)
            {
                randomPositions.Add(newRoomPosition);
                continue;
            }

            if (roomPositions.Contains(newRoomPosition) || randomPositions.Contains(newRoomPosition))
            {
                continue;
            }

            roomPositions.Add(newRoomPosition);
            GameObject otherRoomObj = gen.GenerateRoom(newRoomPosition);
            roomStats otherRoomStat = otherRoomObj.GetComponent<roomStats>();
            connector.GetComponent<ConnectorStat>().connected = true;
            starterRoom.Connect(otherRoomObj);
            otherRoomStat.Connect(startingRoom);
            starterRoom.connectedDirs |= originalDir;
            otherRoomStat.connectedDirs |= DirectionalMovement.ReverseDirection(originalDir);
            otherRoomStat.roomDepth = starterRoom.roomDepth + 1;
            otherRoomObj.transform.SetParent(Board.transform);
            generatedRooms.Add(otherRoomObj);

            GameObject ourDoor = null;
            GameObject otherDoor = null;
            foreach (GameObject door in starterRoom.doors)
            {
                DoorStats doorStat = door.GetComponent<DoorStats>();
                if (doorStat.dir != originalDir)
                {
                    continue;
                }
                doorStat.connectingRoom = otherRoomObj;
                ourDoor = door;
            }
            foreach (GameObject door in otherRoomStat.doors)
            {
                DoorStats doorStat = door.GetComponent<DoorStats>();
                if (doorStat.dir != DirectionalMovement.ReverseDirection(originalDir))
                {
                    continue;
                }
                doorStat.connectingRoom = startingRoom;
                otherDoor = door;
            }

            ourDoor.GetComponent<DoorStats>().otherDoorObj = otherDoor;
            otherDoor.GetComponent<DoorStats>().otherDoorObj = ourDoor;

            if (otherRoomStat.roomDepth < maxRoomDepth)
            {
                BranchOut(otherRoomObj);
            }


        }
    }

    void SealDungeon()
    {
        foreach (GameObject room in generatedRooms)
        {
            List<GameObject> connectors = SearchChildren.SearchForTag(room, "Connector");
            List<GameObject> doors = SearchChildren.SearchForTag(room, "Door");
            foreach (GameObject connector in connectors)
            {
                direction roomDir = room.GetComponent<roomStats>().connectedDirs;
                direction connectorDir = connector.GetComponent<ConnectorStat>().dir;
                if (roomDir.HasFlag(connectorDir))
                {
                    continue;
                }

                direction reverseDir = DirectionalMovement.ReverseDirection(connectorDir);
                Vector3 wallPosition = DirectionalMovement.MoveTo(reverseDir, connector.transform.position);
                GameObject wallToGenerate = gen.wallArray[Random.Range(0, gen.wallArray.Length)];

                GameObject wall = Instantiate(wallToGenerate, wallPosition, Quaternion.identity);
                Transform wallHolder = room.transform.GetChild(0);
                wall.transform.SetParent(wallHolder);
                foreach (GameObject door in doors)
                {
                    if (door.GetComponent<DoorStats>().dir == connectorDir)
                    {
                        room.GetComponent<roomStats>().doors.Remove(door);
                        Destroy(door);
                    }
                }
            }
        }
    }
    void HideDungeon()
    {
        foreach (GameObject room in generatedRooms)
        {
            if (room == generatedRooms[0])
            {
                continue;
            }
            room.SetActive(false);
        }
    }
}
public enum RoomSize
{
    SMALL = 6,
    MEDIUM = 8,
    BIG = 10,
    HUGE = 12
}
