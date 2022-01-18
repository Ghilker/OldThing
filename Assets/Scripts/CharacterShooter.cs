using UnityEngine;
using static Helper.MouseCamera;

public class CharacterShooter : MonoBehaviour
{
    private Transform shootingPivot;
    private Transform shootingPoint;

    private void Start()
    {
        shootingPivot = transform.GetChild(0);
        shootingPoint = shootingPivot.GetChild(0);
    }

    void Update()
    {
        Vector3 MousePosition = GetMouseWorldPosition();
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(shootingPivot.transform.position.y - MousePosition.y, shootingPivot.transform.position.x - MousePosition.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        shootingPivot.rotation = Quaternion.Euler(0, 0, AngleDeg + 180f);
    }
}