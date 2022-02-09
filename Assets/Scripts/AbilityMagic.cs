using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MagicBasic")]
public class AbilityMagic : AbilityBase
{
    Transform magicShootingPoint;
    public GameObject magicBullet;
    public string magicBulletTarget = "Monster";

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
        GameObject instantiatedBullet = Instantiate(magicBullet, magicShootingPoint.position + magicShootingPoint.right * 0.5f, Quaternion.identity);
        instantiatedBullet.GetComponent<MagicBulletStats>().bulletTarget = magicBulletTarget;
        Rigidbody2D bulletRb = instantiatedBullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(magicShootingPoint.right.normalized * bulletForce, ForceMode2D.Impulse);
    }
}