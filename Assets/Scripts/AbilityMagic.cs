using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MagicBasic")]
public class AbilityMagic : AbilityBase
{
    Transform magicShootingPoint;
    public GameObject magicBullet;

    public float bulletForce = 0f;

    public override void InitializeAbility(Transform abilityHolder)
    {
        magicShootingPoint = abilityHolder;
        abilityAnimationController = abilityHolder.GetComponent<Animator>();
    }
    public override void TriggerAbility()
    {
        UseMagic();
    }

    public virtual void UseMagic()
    {
        GameObject instantiatedBullet = Instantiate(magicBullet, magicShootingPoint.position, Quaternion.identity);
        Rigidbody bulletRb = instantiatedBullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(magicShootingPoint.forward.normalized * bulletForce, ForceMode.Impulse);
    }
}