using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    GameObject player;

    public int currentRoomWidth = 10;
    public int currentRoomHeight = 10;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}