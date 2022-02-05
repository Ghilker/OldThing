using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Attack/Magic")]
public class MagicAttackAI : AttackAI
{
    public GameObject bulletPrefab;
    public string magicBulletTarget = "Player";

    public override void Act(MonsterStats stats)
    {
        Attack(stats);
    }

    private void Attack(MonsterStats stats)
    {
        GameObject instantiatedBullet = Instantiate(bulletPrefab, stats.eyes.position, Quaternion.identity);
        Rigidbody bulletRb = instantiatedBullet.GetComponent<Rigidbody>();
        instantiatedBullet.GetComponent<MagicBulletStats>().bulletTarget = magicBulletTarget;
        bulletRb.AddForce(stats.eyes.forward.normalized * stats.magicAttackSpeed, ForceMode.Impulse);
    }
}