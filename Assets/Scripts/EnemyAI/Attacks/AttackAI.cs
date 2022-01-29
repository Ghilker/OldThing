using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAI : ScriptableObject
{
    public AbilityType damageType;

    public abstract void Act(MonsterStats stats);
}
