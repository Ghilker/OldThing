using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MeleeBasic")]
public class AbilityMelee : AbilityBase
{

    Transform attackPoint;
    public Vector2 attackDistance = new Vector2(0.5f, 0f);
    public float attackSize = 0.3f;

    public override void InitializeAbility(Transform abilityHolder)
    {
        attackPoint = abilityHolder;
        abilityType = AbilityType.MELEE;
        abilityHolder.localPosition = attackDistance;
        abilityHolder.GetComponent<CircleCollider2D>().radius = attackSize;
    }

    public override void TriggerAbility()
    {
        CircleCollider2D pointOfAttack = attackPoint.GetComponent<CircleCollider2D>();
        UseAbility(pointOfAttack);
    }

    public virtual void UseAbility(CircleCollider2D pointOfAttack)
    {
        pointOfAttack.enabled = true;
        PerformMeleeCheck();
        CoroutineHelper.instance.StartCoroutine(MeleeCooldown(pointOfAttack));
    }

    public virtual IEnumerator MeleeCooldown(CircleCollider2D pointOfAttack)
    {
        yield return new WaitForSeconds(abilityCooldown);
        pointOfAttack.enabled = false;
    }

    public virtual void PerformMeleeCheck()
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(attackPoint.position, attackSize);
        foreach (Collider2D colliderHit in collidersHit)
        {
            if (colliderHit.tag != "Monster")
            {
                continue;
            }
            Destroy(colliderHit.gameObject);
        }
    }
}