using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    Animator anim;
    float hf = 0.0f;
    float vf = 0.0f;


    [SerializeField]
    private float speed = 10f;

    public bool teleporting = false;
    Vector3 movement;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        hf = 0;
        vf = movement.y > 0.01f ? movement.y : movement.y < -0.01f ? 1 : 0;
        if (movement.x < -0.01f)
        {
            hf = -1;
        }
        if (movement.x > 0.01f)
        {
            hf = 1;
        }

        anim.SetFloat("Horizontal", hf);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", vf);

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
