using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class DoorStats : MonoBehaviour
{
    public direction dir;
    public GameObject connectingRoom;
    public GameObject otherDoorObj;

    public void DoorDisable()
    {
        foreach (GameObject otherDoor in connectingRoom.GetComponent<roomStats>().doors)
        {
            if (otherDoor.GetComponent<DoorStats>().dir != DirectionalMovement.ReverseDirection(dir))
            {
                continue;
            }
            otherDoor.GetComponent<SpriteRenderer>().enabled = false;
            otherDoor.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void DoorEnable()
    {
        foreach (GameObject otherDoor in connectingRoom.GetComponent<roomStats>().doors)
        {
            if (otherDoor.GetComponent<DoorStats>().dir != DirectionalMovement.ReverseDirection(dir))
            {
                continue;
            }
            otherDoor.GetComponent<SpriteRenderer>().enabled = true;
            otherDoor.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            connectingRoom.SetActive(true);
            connectingRoom.GetComponent<roomStats>().isActive = true;
            other.GetComponent<CharacterMovement>().enabled = false;
            DoorDisable();
            Vector3 playerPosition = DirectionalMovement.MoveTo(dir, otherDoorObj.transform.position);
            other.gameObject.transform.position = playerPosition;
            other.GetComponent<CharacterMovement>().enabled = true;
            UpdateCamera();
            RoomHide(otherDoorObj.GetComponent<DoorStats>().connectingRoom, connectingRoom);
        }
    }

    void UpdateCamera()
    {
        Vector3 cameraPosition = new Vector3((connectingRoom.GetComponent<roomStats>().width + 1) / 2, (connectingRoom.GetComponent<roomStats>().height + 1) / 2, -10f);
        Camera.main.transform.position = connectingRoom.transform.position + cameraPosition + new Vector3(0.5f, 0.5f, 0f);
        Camera.main.GetComponent<CameraMovements>().currentRoomWidth = connectingRoom.GetComponent<roomStats>().width;
        Camera.main.GetComponent<CameraMovements>().currentRoomHeight = connectingRoom.GetComponent<roomStats>().height;
        Camera.main.GetComponent<CameraMovements>().currentRoomCoordinates = connectingRoom.transform.position;
    }

    void RoomHide(GameObject oldRoom, GameObject newRoom)
    {
        //oldRoom.SetActive(false);
        List<GameObject> oldObjects = SearchChildren.AllChilds(oldRoom);
        foreach (GameObject oldObject in oldObjects)
        {
            if (!oldObject.GetComponent<SpriteRenderer>())
            {
                continue;
            }
            Color color = oldObject.GetComponent<SpriteRenderer>().color;
            color.r = 0.5f;
            color.g = 0.5f;
            color.b = 0.5f;
            oldObject.GetComponent<SpriteRenderer>().color = color;
        }
        newRoom.SetActive(true);
        List<GameObject> newObjects = SearchChildren.AllChilds(newRoom);
        foreach (GameObject newObject in newObjects)
        {
            if (!newObject.GetComponent<SpriteRenderer>())
            {
                continue;
            }
            Color color = newObject.GetComponent<SpriteRenderer>().color;
            color.r = 1f;
            color.g = 1f;
            color.b = 1f;
            newObject.GetComponent<SpriteRenderer>().color = color;
        }

    }
}
