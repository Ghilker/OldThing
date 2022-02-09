using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Backpack")]
public class Backpack : ScriptableObject
{
    [SerializeField] int keys = 0;
    public int GetKeys
    {
        get => keys;
    }
    public void UpdateKeys(int numberOfKeys)
    {
        keys += numberOfKeys;
    }

    public bool RemoveKey()
    {
        if (keys > 0)
        {
            keys--;
            return true;
        }
        return false;
    }

}