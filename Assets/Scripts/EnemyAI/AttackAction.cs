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

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);
        controller.navMeshAgent.isStopped = true;
        if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange)
            && hit.collider.CompareTag("Player"))
        {
            if (controller.CheckIfCountDownElapsed(controller.enemyStats.attackRate))
            {
                Debug.Log("HIT");
                controller.stateTimeElapsed = 0;
            }
        }
    }
}