using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    private GameObject target;
    [SerializeField]
    private float speed;
    private MonsterStats stats;
    Vector3Int movement;
    private float lastStep = 0;
    private float waitTime = 2f;
    private Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<MonsterStats>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (lastStep < Time.time)
        {
            movement.x = Mathf.Clamp((int)(target.transform.position.x - transform.position.x), -1, 1);

            movement.y = Mathf.Clamp((int)(target.transform.position.y - transform.position.y), -1, 1);
            if (movement.x != 0 && AttemptMove(transform.position + Vector3.one * 0.5f, (transform.position + Vector3.one * 0.5f) + new Vector3(movement.x, 0f, 0f)))
            {
                movement.y = 0;
            }
            else if (movement.y != 0 && AttemptMove(transform.position + Vector3.one * 0.5f, (transform.position + Vector3.one * 0.5f) + new Vector3(0f, movement.y, 0f)))
            {
                movement.x = 0;
            }

            if (AttemptMove(transform.position + Vector3.one * 0.5f, (transform.position + Vector3.one * 0.5f) + movement))
            {
                GetComponent<CircleCollider2D>().enabled = false;
                transform.position += movement;
                GetComponent<CircleCollider2D>().enabled = true;
            }
            lastStep = Time.time + waitTime;

        }
    }

    bool AttemptMove(Vector3 currentPosition, Vector3 nextPosition)
    {
        bool canMove = true;
        GetComponent<CircleCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(currentPosition, nextPosition);
        GetComponent<CircleCollider2D>().enabled = true;
        if (hit.transform != null)
        {
            if (hit.transform.gameObject.tag != "Door" || hit.transform.gameObject.tag != "Player")
            {
                canMove = false;
            }

        }
        return canMove;
    }

    public void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
