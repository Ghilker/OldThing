using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.maxAttackRange, Color.red);
        controller.navMeshAgent.isStopped = true;
        if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.maxAttackRange)
            && hit.collider.CompareTag("Player"))
        {
            if (controller.CheckIfCountDownElapsed(controller.enemyStats.attackRate))
            {
                controller.enemyStats.PerformAttack();
                controller.stateTimeElapsed = 0;
            }
        }
    }
}