using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public bool teleporting = false;
    Vector2 movement;
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = movement * speed;
    }

    IEnumerator Teleport()
    {
        teleporting = true;
        yield return new WaitForSeconds(1);
        teleporting = false;
    }

}
