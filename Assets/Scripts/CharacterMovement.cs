using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public bool teleporting = false;

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        GetComponent<Rigidbody2D>().velocity = movement * speed;
    }

    IEnumerator Teleport()
    {
        teleporting = true;
        yield return new WaitForSeconds(1);
        teleporting = false;
    }

}
