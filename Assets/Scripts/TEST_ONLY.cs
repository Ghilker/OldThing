using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class TEST_ONLY : MonoBehaviour
{
    public RoomGenerator gen;

    private void Start()
    {
        gen.GenerateRoom(Vector2.zero);
    }
}
