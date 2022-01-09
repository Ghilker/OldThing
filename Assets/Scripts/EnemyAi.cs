using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    private GameObject target;
    [SerializeField]
    private float speed;
    private MonsterStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<MonsterStats>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!stats.isAlive)
        {
            return;
        }
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
