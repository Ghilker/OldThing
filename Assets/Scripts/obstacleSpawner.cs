using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleSpawner : MonoBehaviour
{
    public List<GameObject> rockObstacles;
    public bool square = false;
    // Start is called before the first frame update
    void Start()
    {
        Transform parent = transform.parent;
        GameObject toInstantiate = rockObstacles[Random.Range(0, rockObstacles.Count)];
        GameObject instantiated = Instantiate(toInstantiate, transform.position, Quaternion.identity);
        instantiated.transform.SetParent(parent);
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {

        if (!square)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + new Vector3(.5f, .5f, 0f), 0.5f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(.5f, .5f, 0f), Vector3.one);
        }


    }
}
