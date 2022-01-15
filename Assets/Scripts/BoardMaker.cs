using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using static Helper.DirectionalMovement;

public class BoardMaker : MonoBehaviour
{
    [SerializeField]
    //All the rooms made in the current level
    List<GameObject> generatedRooms;

    [SerializeField]
    //List of vector3 that indicates the generated rooms positions
    List<Vector2> roomGridPositions = new List<Vector2>();

    //List of the random empty spots in the grid
    List<Vector2> randomPositions = new List<Vector2>();

    public GameObject[] emptyRoomsArrayHigh;
    public GameObject[] emptyRoomsArrayMid;
    public GameObject[] emptyRoomsArrayLow;
    public GameObject intialRoom;

    public GameObject player;
    public GameObject closingWallObj;
    GameObject bossRoom;

    //Main holder for all game objects
    GameObject Board;
    //Max dept that the rooms can reach
    int maxRoomDepth = 4;

    //Reference to the camera
    Camera mainCamera;

    List<GameObject> specialRooms = new List<GameObject>();

    public GameObject monsterSpawner;

    public void BoardInit(int dungeonDepth)
    {
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
        //PopulateDungeon();
        HideDungeon();
    }

    void CreateRoom()
    {
        //Creation of the first room
        generatedRooms = new List<GameObject>();
        GameObject firstRoom = Instantiate(intialRoom, Vector3.zero, Quaternion.identity);
        firstRoom.transform.SetParent(Board.transform);
        generatedRooms.Add(firstRoom);
        roomStats firstRoomStats = firstRoom.GetComponent<roomStats>();
        firstRoomStats.isSpecial = true;
        firstRoomStats.isActive = true;
        int middleX = firstRoomStats.width / 2;
        int middleY = firstRoomStats.height / 2;
        //Adjusting position of the camera to the center of the first room
        Vector3 cameraPosition = new Vector3((firstRoomStats.width + 1) / 2, (firstRoomStats.height + 1) / 2, -10f);
        mainCamera.transform.position = cameraPosition + new Vector3(0.5f, 0.5f, 0f);
        mainCamera.GetComponent<CameraMovements>().currentRoomWidth = firstRoomStats.width;
        mainCamera.GetComponent<CameraMovements>().currentRoomHeight = firstRoomStats.height;
        Instantiate(player, new Vector3(middleX, middleY, 0f), Quaternion.identity);
        foreach (Vector2 newRoomLocalGridPosition in firstRoomStats.internalGrid)
        {
            Vector2 worldGridPosition = newRoomLocalGridPosition;
            if (!roomGridPositions.Contains(worldGridPosition))
                roomGridPositions.Add(worldGridPosition);
        }
        if (maxRoomDepth < 1)
        {
            return;
        }
        BranchOut(firstRoom);
    }

    void BranchOut(GameObject oldRoom)
    {
        roomStats oldRoomStats = oldRoom.GetComponent<roomStats>();
        List<GameObject> connectors = SearchChildren.SearchForTag(oldRoom, "Connector");
        List<GameObject> ourDoors = SearchChildren.SearchForTag(oldRoom, "Door");
        oldRoomStats.doors = ourDoors;
        RandomHelper.ShuffleList(connectors);
        foreach (GameObject currentConnector in connectors)
        {
            ConnectorStat currentConnectorStats = currentConnector.GetComponent<ConnectorStat>();
            Vector2 newRoomPositionGrid = MoveTo(currentConnectorStats.dir, oldRoomStats.roomCoordinates + currentConnectorStats.localGridPosition);
            if (roomGridPositions.Contains(newRoomPositionGrid))
            {
                continue;
            }
            GameObject pickedRoom = PickRoom(newRoomPositionGrid);
            Vector3 newRoomPosition = new Vector3(newRoomPositionGrid.x * 28, newRoomPositionGrid.y * 28);
            GameObject instantiatedRoom = Instantiate(pickedRoom, newRoomPosition, Quaternion.identity);
            roomStats instantiatedRoomStats = instantiatedRoom.GetComponent<roomStats>();
            instantiatedRoomStats.roomCoordinates = newRoomPositionGrid;
            instantiatedRoomStats.roomDepth = oldRoomStats.roomDepth + 1;
            instantiatedRoomStats.isActive = true;
            instantiatedRoom.transform.SetParent(Board.transform);
            foreach (Vector2 newRoomLocalGridPosition in instantiatedRoomStats.internalGrid)
            {
                Vector2 worldGridPosition = newRoomLocalGridPosition + newRoomPositionGrid;
                if (!roomGridPositions.Contains(worldGridPosition))
                    roomGridPositions.Add(worldGridPosition);
                else
                    Debug.Log("Error, already containing position " + newRoomLocalGridPosition);
            }
            generatedRooms.Add(instantiatedRoom);

            List<GameObject> otherDoors = SearchChildren.SearchForTag(instantiatedRoom, "Door");
            List<GameObject> otherConnectors = SearchChildren.SearchForTag(instantiatedRoom, "Connector");
            GameObject doorToConnect = null;
            Vector2 doorToConnectGridPosition = Vector2.zero;
            foreach (GameObject ourDoor in ourDoors)
            {
                Vector3 ourDoorPosition = MoveTo(ReverseDirection(currentConnectorStats.dir), currentConnector.transform.position);
                if (ourDoor.transform.position != ourDoorPosition)
                {
                    continue;
                }
                doorToConnectGridPosition = currentConnectorStats.localGridPosition + oldRoomStats.roomCoordinates;
                DoorStats ourDoorStats = ourDoor.GetComponent<DoorStats>();
                ourDoorStats.connectingRoom = instantiatedRoom;
                doorToConnect = ourDoor;
            }

            foreach (GameObject otherDoor in otherDoors)
            {
                Vector2 otherDoorGridPosition = MoveTo(currentConnectorStats.dir, oldRoomStats.roomCoordinates + currentConnectorStats.localGridPosition);
                if (otherDoor.GetComponent<DoorStats>().dir == ReverseDirection(doorToConnect.GetComponent<DoorStats>().dir))
                {
                    if (otherDoor.GetComponent<DoorStats>().dir == direction.NORTH || otherDoor.GetComponent<DoorStats>().dir == direction.SOUTH)
                    {
                        if (otherDoorGridPosition.x == doorToConnectGridPosition.x)
                        {
                            otherDoor.GetComponent<DoorStats>().connectingRoom = oldRoom;
                            otherDoor.GetComponent<DoorStats>().otherDoorObj = doorToConnect;
                            doorToConnect.GetComponent<DoorStats>().connectingRoom = instantiatedRoom;
                            doorToConnect.GetComponent<DoorStats>().otherDoorObj = otherDoor;
                            break;
                        }
                    }
                    else if (otherDoor.GetComponent<DoorStats>().dir == direction.EAST || otherDoor.GetComponent<DoorStats>().dir == direction.WEST)
                    {
                        if (otherDoorGridPosition.y == doorToConnectGridPosition.y)
                        {
                            otherDoor.GetComponent<DoorStats>().connectingRoom = oldRoom;
                            otherDoor.GetComponent<DoorStats>().otherDoorObj = doorToConnect;
                            doorToConnect.GetComponent<DoorStats>().connectingRoom = instantiatedRoom;
                            doorToConnect.GetComponent<DoorStats>().otherDoorObj = otherDoor;
                            break;
                        }
                    }
                }
            }

            if (instantiatedRoomStats.roomDepth < maxRoomDepth)
            {
                BranchOut(instantiatedRoom);
            }

        }
    }

    GameObject PickRoom(Vector2 gridPosition)
    {
        GameObject pickedRoom = null;
        float[] weights = { 65, 25, 10 };
        int arrayToPick = RandomHelper.GetRandomWeightedIndex(weights);
        switch (arrayToPick)
        {
            case (0):
                pickedRoom = emptyRoomsArrayHigh[Random.Range(0, emptyRoomsArrayHigh.Length)];
                break;
            case (1):
                pickedRoom = emptyRoomsArrayMid[Random.Range(0, emptyRoomsArrayMid.Length)];
                break;
            case (2):
                pickedRoom = emptyRoomsArrayLow[Random.Range(0, emptyRoomsArrayLow.Length)];
                break;
        }

        roomStats pickedRoomStats = pickedRoom.GetComponent<roomStats>();
        foreach (Vector2 localGridPosition in pickedRoomStats.internalGrid)
        {
            Vector2 worldGridPosition = localGridPosition + gridPosition;
            if (roomGridPositions.Contains(worldGridPosition))
            {
                return PickRoom(gridPosition);
            }
        }
        return pickedRoom;
    }

    void SealDungeon()
    {
        foreach (GameObject currentRoom in generatedRooms)
        {
            List<GameObject> currentDoors = SearchChildren.SearchForTag(currentRoom, "Door");
            foreach (GameObject currentDoor in currentDoors)
            {
                if (currentDoor.GetComponent<DoorStats>().connectingRoom == null)
                {
                    GameObject closingWall = Instantiate(closingWallObj, currentDoor.transform.position, Quaternion.identity);
                    closingWall.transform.SetParent(currentRoom.transform.GetChild(0));
                    Destroy(currentDoor);
                }
            }
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

    /*void BranchOut(GameObject startingRoom)
    {
        List<GameObject> connectors = SearchChildren.SearchForTag(startingRoom, "Connector");
        List<GameObject> doors = SearchChildren.SearchForTag(startingRoom, "Door");
        roomStats startingRoomStats = startingRoom.GetComponent<roomStats>();
        List<direction> directions = new List<direction>() { direction.NORTH, direction.EAST, direction.SOUTH, direction.WEST };
        RandomHelper.ShuffleList(directions);
        if (RandomHelper.prob(25))
        {
            int randomToRemove = Random.Range(0, directions.Count);
            directions.Remove(directions[randomToRemove]);
        }
        if (RandomHelper.prob(25) && startingRoomStats.roomDepth > 2)
        {
            int randomToRemove = Random.Range(0, directions.Count);
            directions.Remove(directions[randomToRemove]);
            if (RandomHelper.prob(5) && startingRoomStats.roomDepth > 4)
            {
                randomToRemove = Random.Range(0, directions.Count);
                directions.Remove(directions[randomToRemove]);
            }
        }

        foreach (direction dir in directions)
        {

            if (startingRoomStats.connectedDirs.HasFlag(dir))
            {
                continue;
            }

            Vector3 currentPosition = startingRoom.transform.position;
            direction originalDir = dir;
            int currentDepth = startingRoomStats.roomDepth;

            Vector3 newRoomPosition = DirectionalMovement.GetVectorOffsetInDir(originalDir, currentPosition, 5);
            Vector2 newRoomPositionGrid = DirectionalMovement.MoveTo(dir, startingRoomStats.roomCoordinates);
            if (RandomHelper.prob(50) && startingRoomStats.roomDepth > 2)
            {
                randomPositions.Add(newRoomPositionGrid);
                continue;
            }

            if (roomGridPositions.Contains(newRoomPositionGrid) || randomPositions.Contains(newRoomPositionGrid))
            {
                continue;
            }

            roomGridPositions.Add(newRoomPositionGrid);
            pickedGenerator = roomGenerators[Random.Range(0, roomGenerators.Length)];
            GameObject otherRoomObj = pickedGenerator.GenerateRoom(newRoomPosition);
            roomStats otherRoomStat = otherRoomObj.GetComponent<roomStats>();
            otherRoomStat.roomGenerator = pickedGenerator;
            List<GameObject> otherConnectors = SearchChildren.SearchForTag(otherRoomObj, "Connector");
            foreach (GameObject connector in otherConnectors)
            {
                if (connector.GetComponent<ConnectorStat>().dir == dir)
                {
                    connector.GetComponent<ConnectorStat>().connected = true;
                }
            }

            otherRoomStat.roomCoordinates = newRoomPositionGrid;
            startingRoomStats.connectedDirs |= originalDir;
            otherRoomStat.connectedDirs |= DirectionalMovement.ReverseDirection(originalDir);
            otherRoomStat.roomDepth = startingRoomStats.roomDepth + 1;
            otherRoomStat.isActive = true;
            otherRoomObj.transform.SetParent(Board.transform);
            generatedRooms.Add(otherRoomObj);

            GameObject ourDoor = null;
            GameObject otherDoor = null;
            foreach (GameObject door in startingRoomStats.doors)
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
    }*/
}
