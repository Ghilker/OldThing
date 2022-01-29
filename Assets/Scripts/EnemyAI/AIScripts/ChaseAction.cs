using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = true;
        Vector3 targetDirection = controller.chaseTarget.position - controller.eyesPivot.position;
        Vector3 newRotation = Vector3.RotateTowards(controller.eyesPivot.forward, targetDirection, controller.enemyStats.searchingTurnSpeed * Time.deltaTime, 0f);
        controller.eyesPivot.rotation = Quaternion.LookRotation(newRotation);
        controller.navMeshAgent.isStopped = false;
    }
}