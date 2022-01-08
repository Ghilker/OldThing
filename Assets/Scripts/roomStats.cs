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
    public List<direction> connectedDirections;
    public Dictionary<direction, GameObject> neighboorRooms;
    public List<GameObject> doors;
    [EnumFlagsAttribute] public direction connectedDirs;
    public int roomDepth = 0;


    public void Connect(GameObject otherRoom)
    {
        if (neighboorRooms == null)
        {
            neighboorRooms = new Dictionary<direction, GameObject>();
        }
        direction dir = direction.NORTH;
        dir = DirectionalMovement.CheckVectorialDirection(roomCoordinates, otherRoom.GetComponent<roomStats>().roomCoordinates);
        neighboorRooms.Add(dir, otherRoom);
        connectedRooms.Add(otherRoom);
        connectedDirections.Add(dir);
    }
}
