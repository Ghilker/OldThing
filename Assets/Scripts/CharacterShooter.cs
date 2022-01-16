using UnityEngine;

public class CharacterShooter : MonoBehaviour
{

    public GameObject _projectile;

    private Transform shootingPivot;

    private Transform shootingPoint;

    private float elapsedTime = 0f;

    private void Start()
    {
        shootingPivot = transform.GetChild(0);

        shootingPoint = shootingPivot.GetChild(0);
    }

    void Update()
    {

        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(shootingPivot.transform.position.y - MousePosition.y, shootingPivot.transform.position.x - MousePosition.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        shootingPivot.rotation = Quaternion.Euler(0, 0, AngleDeg + 180f);

        if (Input.GetMouseButton(0) && elapsedTime < Time.time)
        {
            GameObject instantiatedBullet = Instantiate(_projectile, shootingPoint.position, shootingPivot.rotation);
            instantiatedBullet.GetComponent<Rigidbody2D>().velocity += (Vector2)shootingPivot.right * 2;
            elapsedTime = Time.time + 0.5f;
        }
    }
}