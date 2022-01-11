using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    GameObject player;

    int baseWidth = 10;
    int baseHeight = 10;
    public int currentRoomWidth = 10;
    public int currentRoomHeight = 10;
    public Vector3 currentRoomCoordinates;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        bool moveVertical = false;
        if (currentRoomHeight > baseHeight)
        {
            moveVertical = true;
        }
        bool moveHorizontal = false;
        if (currentRoomWidth > baseWidth)
        {
            moveHorizontal = true;
        }
        int vertical = 0;
        if (moveVertical)
        {
            vertical = (currentRoomWidth - (currentRoomWidth / 4)) / 2;
        }
        int horizontal = 0;
        if (moveHorizontal)
        {
            horizontal = (currentRoomHeight - (currentRoomHeight / 4)) / 2;
        }
        Vector3 offset = new Vector3(0f, 0f, -10f);
        Vector3 allowedMovement = new Vector3();
        allowedMovement.x = Mathf.Clamp(player.transform.position.x, currentRoomCoordinates.x + (currentRoomWidth / 2 + .5f) - horizontal, currentRoomCoordinates.x + (currentRoomWidth / 2 + .5f) + horizontal);
        allowedMovement.y = Mathf.Clamp(player.transform.position.y, currentRoomCoordinates.y + (currentRoomHeight / 2 + .5f) - vertical, currentRoomCoordinates.y + (currentRoomHeight / 2 + .5f) + vertical);
        transform.position = allowedMovement + offset;

    }

}