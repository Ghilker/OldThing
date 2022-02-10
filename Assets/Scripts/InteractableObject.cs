using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractableObject : MonoBehaviour
{

    public GameObject connectedObject;
    public InteractAction interactAction;

    public NavMeshSurface2d nav;

    public bool requireKey = false;
    public bool updateNavmesh = false;

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

        if (updateNavmesh)
        {
            nav.BuildNavMesh();
        }

    }
}
