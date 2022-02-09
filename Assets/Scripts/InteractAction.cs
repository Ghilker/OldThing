using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractAction : ScriptableObject
{
    public abstract void Trigger(GameObject triggeredObj);
}