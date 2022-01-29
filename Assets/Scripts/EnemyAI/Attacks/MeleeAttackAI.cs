using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Attack/Melee")]
public class MeleeAttackAI : AttackAI
{
    public override void Act(MonsterStats stats)
    {
        Attack(stats);
    }

    private void Attack(MonsterStats stats)
    {
        RaycastHit hit;
        if (Physics.SphereCast(stats.eyes.position, stats.lookSphereCastRadius, stats.eyes.forward, out hit, stats.maxAttackRange) && hit.collider.CompareTag("Player"))
        {
            Debug.Log("PlayerHIT");
        }
    }
}
