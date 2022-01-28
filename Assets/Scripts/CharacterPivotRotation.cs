using UnityEngine;
using static Helper.MouseCamera;

public class CharacterPivotRotation : MonoBehaviour
{
    public Transform rotatingPivot;
    public Transform armPoint;
    public bool canRotate = true;

    void Update()
    {
        if (!canRotate)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Vector3 hitPosition = hit.point;
            hitPosition.y = rotatingPivot.position.y;
            rotatingPivot.LookAt(hitPosition);
        }
    }
}