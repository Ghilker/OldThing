using UnityEngine;
using static Helper.MouseCamera;

public class CharacterPivotRotation : MonoBehaviour
{
    public Transform weapon;
    public bool canRotate = true;

    void Update()
    {
        if (!canRotate)
        {
            return;
        }

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weapon.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}