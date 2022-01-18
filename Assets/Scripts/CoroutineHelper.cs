using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    public static CoroutineHelper instance;

    void Start()
    {
        CoroutineHelper.instance = this;
    }
}