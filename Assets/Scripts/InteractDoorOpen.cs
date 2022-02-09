using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/OpenDoor")]
public class InteractDoorOpen : InteractAction
{
    public override void Trigger(GameObject triggeredObj)
    {
        OpenDoor(triggeredObj);
    }

    void OpenDoor(GameObject doorToOpen)
    {
        Destroy(doorToOpen);
    }

}