using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Helper;

public class DoorStats : MonoBehaviour
{
    public direction dir;
    public GameObject connectingRoom;
    public GameObject otherDoorObj;
    public bool connected = false;

    public void DoorDisable()
    {
        otherDoorObj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        otherDoorObj.GetComponent<BoxCollider>().isTrigger = true;

        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public void DoorEnable()
    {
        otherDoorObj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        otherDoorObj.GetComponent<BoxCollider>().isTrigger = false;

        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
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
        roomStats connectingRoomStats = connectingRoom.GetComponent<roomStats>();
        Vector3 cameraPosition = new Vector3((connectingRoomStats.width + 1) / 2, 10, (connectingRoomStats.height + 1) / 2);
        Camera.main.transform.position = connectingRoom.transform.position + cameraPosition;
    }

    void RoomHide(GameObject oldRoom, GameObject newRoom)
    {
        oldRoom.SetActive(false);
        newRoom.SetActive(true);
    }
}
