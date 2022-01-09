using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject enemy;
    void Start()
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
