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
    public GameObject spriteHolder;

    Vector3 movement;

    Rigidbody rb;
    Vector3 rayOffset = new Vector3(0f, -0.3f, 0f);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = spriteHolder.GetComponent<Animator>();
    }

    void Update()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + rayOffset, .3f, Vector3.down, 0.5f);

        movement.y += Physics.gravity.y * Time.deltaTime;

        foreach (RaycastHit hitCheck in hits)
        {
            if (hitCheck.collider && hitCheck.collider.tag == "Floor")
            {
                movement.y = 0;
                break;
            }
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        hf = 0;
        vf = movement.z > 0.01f ? movement.z : movement.z < -0.01f ? 1 : 0;
        if (movement.x < -0.01f)
        {
            hf = -1;
        }
        if (movement.x > 0.01f)
        {
            hf = 1;
        }
        anim.SetFloat("Horizontal", hf);
        anim.SetFloat("Vertical", movement.z);
        anim.SetFloat("Speed", vf);
    }

    void FixedUpdate()
    {
        movement.Normalize();
        rb.velocity = movement * speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + rayOffset, .3f);
    }
}
