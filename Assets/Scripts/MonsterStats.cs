using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{

    public bool isAlive = true;

    public int lifePoints = 5;

    public float lookRange = 3;
    public float lookSphereCastRadius = 1;
    public float searchingTurnSpeed = 3;
    public float searchDuration = 2;
    public float attackRange = 1;
    public float attackRate = 1.5f;
    public float attackDamage = 2;

    public void TakeDamage(int amount)
    {
        lifePoints -= amount;
        if (lifePoints <= 0)
        {
            Destroy(gameObject);
        }
    }

}
