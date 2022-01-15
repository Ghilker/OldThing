using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject[] enemies;
    public roomStats room;
    public bool canSpawn = true;

    public void SpawnMonsters()
    {
        GameObject pickedEnemy = enemies[Random.Range(0, enemies.Length)];
        GameObject instantiatedEnemy = Instantiate(pickedEnemy, transform.position, Quaternion.identity);
        room.monsters.Add(instantiatedEnemy);
        instantiatedEnemy.transform.SetParent(transform);
        canSpawn = false;

    }
    void OnDrawGizmos()
    {



        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(.5f, .5f, 0f), Vector3.one);



    }
}
