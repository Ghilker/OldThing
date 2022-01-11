using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public bool teleporting = false;
    Vector3 movement;
    private float lastStep = 0;
    private float waitTime = .5f;
    private Rigidbody2D rb2D;
    private float inverseMoveTime = 10f;

    void Update()
    {
        if (lastStep < Time.time)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (movement.x != 0)
            {
                movement.y = 0;
            }
            if (AttemptMove(transform.position + Vector3.one * 0.5f, (transform.position + Vector3.one * 0.5f) + movement))
            {
                transform.position += movement;
            }
            lastStep = Time.time + waitTime;
        }
    }

    bool AttemptMove(Vector3 currentPosition, Vector3 nextPosition)
    {
        bool canMove = true;
        GetComponent<Collider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(currentPosition, nextPosition);
        GetComponent<Collider2D>().enabled = true;
        if (hit.transform != null)
        {
            if (hit.transform.tag != "Door")
            {
                canMove = false;
            }

        }
        return canMove;
    }

    IEnumerator Teleport()
    {
        teleporting = true;
        yield return new WaitForSeconds(1);
        teleporting = false;
    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            rb2D.MovePosition(newPostion);

            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

}
