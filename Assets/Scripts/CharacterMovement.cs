using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    Animator anim;

    [SerializeField]
    private float speed = 10f;

    Vector2 movement;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool moving = movement.magnitude > 0;

        float horizontalMovement = 0;

        if (movement.x < -0.01f)
        {
            horizontalMovement = -1;
        }
        if (movement.x > 0.01f)
        {
            horizontalMovement = 1;
        }

        anim.SetBool("Moving", moving);
        anim.SetFloat("Horizontal", horizontalMovement);
    }

    void FixedUpdate()
    {
        movement.Normalize();
        rb.velocity = movement * speed;
    }
}
