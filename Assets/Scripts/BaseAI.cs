using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Helper;

public class BaseAI : MonoBehaviour
{

    private GameObject playerTarget;
    private GameObject aiHolder;

    private NavMeshAgent aiAgent;

    public string aiFaction;

    public float minFollowDistance;
    public float maxFollowDistance;

    public bool isActive = true;
    public bool isStationary;
    public bool isAttackingPlayer;
    public bool isMoving;
    public bool canAttack;
    public bool canMove;
    public bool canMoveWhileAttacking;
    public bool canTakeDamage;

    public Vector3 moveToPosition;

    public AbilityType abilityType = AbilityType.MELEE;

    public float minAttackDistance;
    public float maxAttackDistance;
    public float minAttackDamage;
    public float maxAttackDamage;

    public virtual void InitializeAI(GameObject aiHolder)
    {
        this.aiHolder = aiHolder;
        this.aiAgent = aiHolder.GetComponent<NavMeshAgent>();
        aiAgent.updateRotation = false;
        aiAgent.updateUpAxis = false;
        playerTarget = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void UpdateAI()
    {
        if (isMoving)
        {
            return;
        }
        if (moveToPosition != Vector3.zero)
        {
            MoveToPosition();
            return;
        }
        float distance = Vector3.Distance(playerTarget.transform.position, aiHolder.transform.position);
        if (distance < minFollowDistance)
        {
            moveToPosition = Helper.NavMeshHelper.RandomNavmeshLocation(0, minFollowDistance, aiHolder);
            return;
        }
        DistanceChecks(distance);
        if (FollowCheck())
        {
            FollowTarget();
        }
        if (AttackCheck())
        {
            AttackTarget();
        }
    }

    public virtual void DistanceChecks(float distance)
    {
        canAttack = false;
        canMove = false;
        if (distance > minAttackDistance && distance < maxAttackDistance)
        {
            canAttack = true;
        }
        if (distance > minFollowDistance && distance < maxFollowDistance)
        {
            canMove = true;
        }
    }

    public virtual void MoveToPosition()
    {
        isMoving = true;
        aiAgent.SetDestination(moveToPosition);
        moveToPosition = Vector3.zero;
        StartCoroutine(WaitForAgentDestination());
    }

    public virtual bool FollowCheck()
    {
        if (!canMove || isStationary || isMoving || (isAttackingPlayer && !canMoveWhileAttacking))
        {
            return false;
        }
        return true;
    }

    public virtual void FollowTarget()
    {
        aiAgent.SetDestination(playerTarget.transform.position);
    }

    public virtual bool AttackCheck()
    {
        if (!canAttack || isAttackingPlayer || isMoving)
        {
            return false;
        }
        return true;
    }

    public virtual void AttackTarget()
    {

    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        UpdateAI();
    }

    IEnumerator WaitForAgentDestination()
    {
        while (aiAgent.pathStatus != NavMeshPathStatus.PathComplete && aiAgent.remainingDistance != 0)
        {
            yield return null;
        }
        isMoving = false;
    }

}