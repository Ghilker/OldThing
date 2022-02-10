using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootAttacker : MonoBehaviour
{
    bool isAttacking;

    string projectileTarget = "Player";

    public Transform player;
    public Transform eyes;
    public GameObject projectilePrefab;

    Vector2 lastSeenPosition;
    float lastSeenTime;

    public float attackTime = 1f;
    public float bulletSpeed = 0.05f;
    float lastAttack;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        lastSeenPosition = transform.position;
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        eyes.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (CheckForObstacle(distance))
        {
            lastSeenTime += Time.deltaTime;
            if (lastSeenTime < 3)
            {
                agent.isStopped = false;
                agent.destination = lastSeenPosition;
            }
            return;
        }

        if (distance > 3 && distance < 5)
        {
            agent.isStopped = false;
            agent.destination = player.position;
        }
        if (distance < 5 && AttackRecharged())
        {
            lastSeenPosition = player.position;
            lastSeenTime = 0;
            agent.isStopped = true;
            PerformAttack();

        }
    }

    void PerformAttack()
    {
        GameObject instantiatedBullet = Instantiate(projectilePrefab, eyes.position + eyes.right * 0.3f, Quaternion.identity);
        instantiatedBullet.GetComponent<MagicBulletStats>().bulletTarget = projectileTarget;
        Rigidbody2D bulletRb = instantiatedBullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(eyes.right.normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    bool AttackRecharged()
    {
        bool canAttack = false;

        lastAttack += Time.deltaTime;

        if (lastAttack >= attackTime)
        {
            canAttack = true;
            lastAttack = 0;
        }

        return canAttack;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, .05f);
    }

    bool CheckForObstacle(float distance)
    {
        bool obstaclePresent = false;

        Vector2 direction = player.position - transform.position;
        direction.Normalize();

        RaycastHit2D[] hits;

        //Debug.DrawRay(transform.position, direction * distance, Color.cyan, 0.001f);

        hits = Physics2D.CircleCastAll(transform.position, 0.05f, direction, distance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Door"))
            {
                obstaclePresent = true;
                break;
            }
        }

        return obstaclePresent;
    }

}
