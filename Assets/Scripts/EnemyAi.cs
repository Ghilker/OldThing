using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    private GameObject target;
    public float attackDistance = 1.6f;

    NavMeshAgent enemyAgent;

    bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAgent.updateRotation = false;
        enemyAgent.updateUpAxis = false;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        if (attacking || !target)
        {
            return;
        }
        float distance = Vector3.Distance(transform.position, target.transform.position);
        var offset = 90f;
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        if (distance > attackDistance)
        {
            enemyAgent.isStopped = false;
            enemyAgent.destination = target.transform.position;
            return;
        }
        enemyAgent.isStopped = true;
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(PrepareAttack());
        }

    }

    IEnumerator PrepareAttack()
    {
        Debug.Log("AAHHHHHH");

        Vector3 currentPosition = transform.position;

        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();

        for (int i = 0; i < 100; i++)
        {
            transform.position -= direction * 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        Debug.Log("TTTACKK!");
        transform.position = currentPosition;
        Attack();

        yield return new WaitForSeconds(0.4f);
        attacking = false;
    }

    void Attack()
    {
        //StartCoroutine(PerformAttack());
    }

    IEnumerator PerformAttack()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        GetComponent<CircleCollider2D>().enabled = false;
    }
}