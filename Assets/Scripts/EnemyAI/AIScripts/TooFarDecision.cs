using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TooFar")]
public class TooFarDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool closeEnough = DistanceCheck(controller);
        return closeEnough;
    }

    private bool DistanceCheck(StateController controller)
    {
        float distance = Vector3.Distance(controller.eyesPivot.position, controller.chaseTarget.position);
        return distance > controller.enemyStats.maxAttackRange + .5f;
    }
}