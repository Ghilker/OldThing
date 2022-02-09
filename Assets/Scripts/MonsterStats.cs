using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    public bool isAlive = true;

    public int lifePoints = 5;

    public float lookRange = 3;

    public float maxAttackRange = 2;
    public float minAttackRange = 1;
    public float attackRate = 1.5f;
    public float attackDamage = 2;

    public float magicAttackSpeed = 2f;

    public Transform eyes;

    float attackTime;
    public float completion;
    public bool isLeaping;

    public void TakeDamage(int amount)
    {
        lifePoints -= amount;
        if (lifePoints <= 0)
        {
            Destroy(gameObject);
        }
    }

}
