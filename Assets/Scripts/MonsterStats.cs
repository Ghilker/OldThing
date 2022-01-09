using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{

    public bool isAlive = true;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HIT");
            isAlive = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }


}
