using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MeleeBasic")]
public class AbilityMelee : AbilityBase
{

    Transform attackPoint;
    //public Vector2 attackDistance = new Vector2(0.5f, 0f);
    public float attackSize = 0.3f;

    public override void InitializeAbility(Transform abilityHolder)
    {
        attackPoint = abilityHolder;
        abilityType = AbilityType.MELEE;
        abilityAnimationController = abilityHolder.GetComponent<Animator>();
    }

    public override void TriggerAbility()
    {
        UseAbility();
    }

    public virtual void UseAbility()
    {
        abilityAnimationController.SetBool(abilityAnimationType, true);
        player.GetComponent<CharacterPivotRotation>().canRotate = false;
        CoroutineHelper.instance.StartCoroutine(PerformMeleeCheck());
        CoroutineHelper.instance.StartCoroutine(MeleeCooldown());
    }

    public virtual IEnumerator MeleeCooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);
        abilityAnimationController.SetBool(abilityAnimationType, false);
        player.GetComponent<CharacterPivotRotation>().canRotate = true;
    }

    public virtual IEnumerator PerformMeleeCheck()
    {
        yield return new WaitForSeconds(0.5f);
        Collider[] collidersHit = Physics.OverlapSphere(attackPoint.position, attackSize);
        foreach (Collider colliderHit in collidersHit)
        {
            if (colliderHit.tag != "Monster")
            {
                continue;
            }
            Destroy(colliderHit.gameObject);
        }
    }
}