using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject nearObject;

    private void Update()
    {
        if (nearObject != null && Input.GetKeyDown(KeyCode.E))
        {
            nearObject.GetComponent<InteractableObject>().TriggerButton(this);
            nearObject = null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            nearObject = other.gameObject;
        }
    }

    public Backpack backpack;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            backpack.UpdateKeys(1);
            Destroy(other.gameObject);
        }
    }

}
