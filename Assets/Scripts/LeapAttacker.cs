using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapAttacker : MonoBehaviour
{
    float completion;
    bool isLeaping;
    public AnimationCurve ease;
    public float height;

    public Transform player;

    public float leapTiming = 1f;
    float lastLeap;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (CheckForObstacle(distance))
        {
            return;
        }

        if (distance > 2 && distance < 3 && !isLeaping)
        {
            agent.isStopped = false;
            agent.destination = player.position;
        }
        if (distance < 2 && !isLeaping && LeapRecharge())
        {
            lastLeap = 0;
            agent.isStopped = true;
            StartCoroutine(LeapAttack(transform.position, player.position, .5f));
        }
    }

    IEnumerator LeapAttack(Vector3 position1, Vector3 position2, float time)
    {
        isLeaping = true;
        float startTime = Time.time;
        while (Time.time - startTime < time)
        {
            completion = ease.Evaluate((Time.time - startTime) / time);
            transform.position = Vector3.Lerp(position1, position2, completion);
            transform.position += Vector3.up * (completion - 1) * -4 * completion * height;
            yield return null;
        }

        isLeaping = false;
    }

    bool LeapRecharge()
    {
        bool canLeap = false;

        lastLeap += Time.deltaTime;

        if (lastLeap > leapTiming)
        {
            canLeap = true;
        }


        return canLeap;
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
            }
        }

        return obstaclePresent;
    }

}