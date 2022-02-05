using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.green);
        Vector3 targetDirection = controller.chaseTarget.position - controller.eyesPivot.position;
        Vector3 newRotation = Vector3.RotateTowards(controller.eyesPivot.forward, targetDirection, controller.enemyStats.searchingTurnSpeed * Time.deltaTime, 0f);
        controller.eyesPivot.rotation = Quaternion.LookRotation(newRotation);

        if (Physics.SphereCast(controller.eyesPivot.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.lookRange)
            && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            return true;
        }
        else
        {
            return false;
        }
    }
}