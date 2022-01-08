using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorStat : MonoBehaviour
{
    [EnumFlagsAttribute] public direction dir;
    public bool connected = false;
}
