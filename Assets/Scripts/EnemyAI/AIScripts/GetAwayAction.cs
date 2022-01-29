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


        controller.navMeshAgent.isStopped = false;
        Vector3 direction = controller.chaseTarget.position - controller.eyesPivot.position;
        direction.Normalize();
        controller.navMeshAgent.destination = controller.eyesPivot.position - direction * controller.enemyStats.minAttackRange;



    }
}