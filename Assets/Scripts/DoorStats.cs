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

        otherDoorObj.GetComponent<SpriteRenderer>().enabled = false;
        otherDoorObj.GetComponent<BoxCollider2D>().isTrigger = true;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void DoorEnable()
    {

        otherDoorObj.GetComponent<SpriteRenderer>().enabled = true;
        otherDoorObj.GetComponent<BoxCollider2D>().isTrigger = false;

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
            //UpdateCamera();
            RoomHide(otherDoorObj.GetComponent<DoorStats>().connectingRoom, connectingRoom);
        }
    }

    void UpdateCamera()
    {
        Vector3 cameraPosition = new Vector3((connectingRoom.GetComponent<roomStats>().width + 1) / 2, (connectingRoom.GetComponent<roomStats>().height + 1) / 2, -10f);
        Camera.main.transform.position = connectingRoom.transform.position + cameraPosition + new Vector3(0.5f, 0.5f, 0f);
    }

    void RoomHide(GameObject oldRoom, GameObject newRoom)
    {
        oldRoom.SetActive(false);
        newRoom.SetActive(true);
    }
}
