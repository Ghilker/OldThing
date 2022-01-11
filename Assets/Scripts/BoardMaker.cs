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
    List<Vector2> roomPositions = new List<Vector2>();

    //List of the random empty spots in the grid
    List<Vector2> randomPositions = new List<Vector2>();

    public RoomGenerator[] roomGenerators;
    //Current room generator
    RoomGenerator pickedGenerator;

    public GameObject player;
    public GameObject testObj;
    GameObject bossRoom;

    //Main holder for all game objects
    GameObject Board;
    //Max dept that the rooms can reach
    public int maxRoomDepth = 4;

    public int specialRoomRarity = 25;
    public int maxSpecialRooms = 3;
    int currentSpecialRooms = 0;

    //Reference to the camera
    Camera mainCamera;

    List<GameObject> specialRooms = new List<GameObject>();

    public GameObject monsterSpawner;

    private void Start()
    {
        pickedGenerator = roomGenerators[Random.Range(0, roomGenerators.Length)];
        mainCamera = Camera.main;
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
        //Creation of the first room
        generatedRooms = new List<GameObject>();
        GameObject firstRoom = pickedGenerator.GenerateRoom(Vector2.zero);
        firstRoom.transform.SetParent(Board.transform);
        generatedRooms.Add(firstRoom);
        roomStats firstRoomStats = firstRoom.GetComponent<roomStats>();
        firstRoomStats.isSpecial = true;
        firstRoomStats.roomCoordinates = Vector2.zero;
        roomPositions.Add(firstRoomStats.roomCoordinates);
        int middleX = firstRoomStats.width / 2;
        int middleY = firstRoomStats.height / 2;
        //Adjusting position of the camera to the center of the first room
        Vector3 cameraPosition = new Vector3((firstRoomStats.width + 1) / 2, (firstRoomStats.height + 1) / 2, -10f);
        mainCamera.transform.position = cameraPosition + new Vector3(0.5f, 0.5f, 0f);
        mainCamera.GetComponent<CameraMovements>().currentRoomWidth = firstRoomStats.width;
        mainCamera.GetComponent<CameraMovements>().currentRoomHeight = firstRoomStats.height;
        mainCamera.GetComponent<CameraMovements>().currentRoomCoordinates = firstRoom.transform.position;
        Instantiate(player, new Vector3(middleX, middleY, 0f), Quaternion.identity);
        BranchOut(firstRoom);
    }

    void BranchOut(GameObject startingRoom)
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

            if (roomPositions.Contains(newRoomPositionGrid) || randomPositions.Contains(newRoomPositionGrid))
            {
                continue;
            }

            roomPositions.Add(newRoomPositionGrid);
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

            startingRoomStats.Connect(otherRoomObj, dir);
            otherRoomStat.Connect(startingRoom, DirectionalMovement.ReverseDirection(dir));
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
                GameObject wallToGenerate = pickedGenerator.wallArray[Random.Range(0, pickedGenerator.wallArray.Length)];

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

    void PopulateDungeon()
    {
        CreateSpecialRooms();
        AddObstacles();
        AddMonsters();
    }

    void CreateSpecialRooms()
    {
        List<GameObject> selectedRooms = new List<GameObject>();
        foreach (GameObject room in generatedRooms)
        {
            if (room.GetComponent<roomStats>().roomDepth < maxRoomDepth)
            {
                continue;
            }
            selectedRooms.Add(room);
        }
        RunSpecialSelection(selectedRooms);

        if (bossRoom)
        {
            Debug.Log("Hello boss");
        }
        else
        {
            Debug.Log("Oh no");
            CreateSpecialRooms();
        }
    }

    void RunSpecialSelection(List<GameObject> selectedRooms)
    {
        foreach (GameObject currentRoom in selectedRooms)
        {
            roomStats currentRoomStats = currentRoom.GetComponent<roomStats>();
            if (!RandomHelper.prob(specialRoomRarity) || currentSpecialRooms >= maxSpecialRooms)
            {
                if (!bossRoom)
                {
                    bossRoom = currentRoom;
                }
                continue;
            }
            if (!bossRoom)
            {
                bossRoom = currentRoom;
            }
            specialRooms.Add(currentRoom);
            currentSpecialRooms++;
            Vector3 middlePoint = new Vector3(currentRoomStats.width / 2, currentRoomStats.height / 2, 0f);
            GameObject specialObj = Instantiate(testObj, currentRoom.transform.position + middlePoint, Quaternion.identity);
            specialObj.transform.SetParent(currentRoom.transform);
            currentRoomStats.isSpecial = true;
            foreach (GameObject door in currentRoomStats.doors)
            {
                GameObject otherDoor = door.GetComponent<DoorStats>().otherDoorObj;
            }
        }
    }

    void AddObstacles()
    {
        foreach (GameObject room in generatedRooms)
        {
            GameObject obstacleHolder = new GameObject("obstacleHolder");
            obstacleHolder.transform.SetParent(room.transform);
            roomStats currentRoomStats = room.GetComponent<roomStats>();
            if (currentRoomStats.isSpecial == true)
            {
                continue;
            }
            List<Vector3> internalRoomGrid = new List<Vector3>();
            foreach (Vector3 position in currentRoomStats.internalGridPositions)
            {
                internalRoomGrid.Add(room.transform.position + position);
            }
            foreach (GameObject door in currentRoomStats.doors)
            {
                Vector3 nearDoor = MoveTo(ReverseDirection(door.GetComponent<DoorStats>().dir), door.transform.position);
                internalRoomGrid.Remove(nearDoor);
                currentRoomStats.availableSpace--;
            }
            RoomGenerator roomGen = currentRoomStats.roomGenerator;
            int randomObstacleAmount = Random.Range(1, 3);
            for (int i = 0; i < randomObstacleAmount; i++)
            {
                Vector3 randomPosition = RandomHelper.RandomPosition(internalRoomGrid);
                GameObject obstacle = Instantiate(roomGen.obstacleArray[Random.Range(0, roomGen.obstacleArray.Length)], randomPosition, Quaternion.identity);
                obstacle.transform.SetParent(obstacleHolder.transform);
                currentRoomStats.obstacles.Add(obstacle);
            }
        }
    }

    void AddMonsters()
    {
        foreach (GameObject room in generatedRooms)
        {
            GameObject monsterHolder = new GameObject("monsterHolder");
            monsterHolder.transform.SetParent(room.transform);
            roomStats currentRoomStats = room.GetComponent<roomStats>();
            if (currentRoomStats.isSpecial == true)
            {
                continue;
            }

            List<Vector3> internalRoomGrid = new List<Vector3>();
            foreach (Vector3 position in currentRoomStats.internalGridPositions)
            {
                internalRoomGrid.Add(room.transform.position + position);
            }
            foreach (GameObject door in currentRoomStats.doors)
            {
                Vector3 nearDoor = MoveTo(ReverseDirection(door.GetComponent<DoorStats>().dir), door.transform.position);
                internalRoomGrid.Remove(nearDoor);
                currentRoomStats.internalGridPositions.Remove(nearDoor);
                currentRoomStats.availableSpace--;
            }
            foreach (GameObject obstacle in currentRoomStats.obstacles)
            {
                internalRoomGrid.Remove(obstacle.transform.position);
                currentRoomStats.availableSpace--;
            }

            RoomGenerator roomGen = currentRoomStats.roomGenerator;
            int randomMonsterAmount = Mathf.Max(0, Random.Range(0, (currentRoomStats.availableSpace + 1) / 2));
            for (int i = 0; i <= randomMonsterAmount; i++)
            {
                Vector3 randomPosition = RandomHelper.RandomPosition(internalRoomGrid);
                GameObject monsterSpawnerInstance = Instantiate(monsterSpawner, randomPosition, Quaternion.identity);
                monsterSpawnerInstance.transform.SetParent(monsterHolder.transform);
                monsterSpawnerInstance.GetComponent<MonsterSpawn>().room = currentRoomStats;
                currentRoomStats.monsterSpawners.Add(monsterSpawnerInstance);
            }
        }
    }
}
