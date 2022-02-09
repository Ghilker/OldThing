using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    public GameObject connectedObject;
    public InteractAction interactAction;

    public bool requireKey = false;

    public void TriggerButton(PlayerController controller)
    {
        bool canTrigger = true;
        if (requireKey && !controller.backpack.RemoveKey())
        {
            canTrigger = false;
        }
        if (canTrigger)
        {
            interactAction.Trigger(connectedObject);
        }

    }
}
