using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Helper;

[CreateAssetMenu(menuName = "PluggableAI/Actions/GetAway")]
public class GetAwayAction : Action
{
    public override void Act(StateController controller)
    {
        GetAway(controller);
    }

    private void GetAway(StateController controller)
    {
        if (controller.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && controller.navMeshAgent.remainingDistance == 0)
        {
            controller.navMeshAgent.destination = NavMeshHelper.RandomPositionAroundCircle(controller.enemyStats.minAttackRange, controller.enemyStats.maxAttackRange, controller.gameObject);
        }

    }
}