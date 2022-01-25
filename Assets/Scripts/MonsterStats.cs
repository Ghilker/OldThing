using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{

    public bool isAlive = true;

    public int lifePoints = 5;
    public int damageDone = 2;

    public void TakeDamage(int amount)
    {
        lifePoints -= amount;
        if (lifePoints <= 0)
        {
            Destroy(gameObject);
        }
    }

}
