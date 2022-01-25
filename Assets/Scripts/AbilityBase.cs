using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{

    public GameObject player;
    public AbilityType abilityType;
    public string abilityName;
    public Sprite abilityImage;
    public Transform abilityHolder;
    public float abilityCooldown = 1f;
    public float abilityDamage;
    public string abilityAnimationType;
    public AnimationClip abilityEffect;
    public Animator abilityAnimationController;

    public abstract void TriggerAbility();
    public abstract void InitializeAbility(Transform abilityHolder);

}