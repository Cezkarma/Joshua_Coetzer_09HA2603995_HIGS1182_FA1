using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Tooltip("Where shots start from. Left empty, the child named ShootingPoint is used.")]
    [SerializeField] private Transform shootingPoint;
    [Tooltip("How far a shot reaches, in world units.")]
    [SerializeField] private float range = 400f;
    [Tooltip("How far down the camera's line of sight aiming starts, so the ship's own hull is never read as the target.")]
    [SerializeField] private float shipClearance = 15f;

    private Camera cam;

    private void Awake()
    {
        if (shootingPoint == null)
        {
            shootingPoint = transform.Find("ShootingPoint");
        }
    }

    private void Start()
    {
        cam = Camera.main;

        if (cam == null || shootingPoint == null)
        {
            Debug.LogWarning("PlayerShooting: needs a camera tagged MainCamera and a ShootingPoint, so shooting is disabled.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        // The left click that re-locks the cursor should not also fire.
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked)
        {
            Fire();
        }
    }

    private void Fire()
    {
        Vector3 origin = shootingPoint.position;
        Vector3 direction = (GetAimPoint() - origin).normalized;

        Debug.DrawRay(origin, direction * range, Color.red, 0.2f);

        if (!Raycast(new Ray(origin, direction), out RaycastHit hit))
        {
            return;
        }

        Asteroid asteroid = hit.collider.GetComponentInParent<Asteroid>();
        if (asteroid != null)
        {
            Debug.Log("Asteroid destroyed by a shot.");
            Destroy(asteroid.gameObject);
        }
    }

    private Vector3 GetAimPoint()
    {
        Ray aim = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        aim.origin = aim.GetPoint(shipClearance);

        return Raycast(aim, out RaycastHit hit) ? hit.point : aim.GetPoint(range);
    }

    private bool Raycast(Ray ray, out RaycastHit hit)
    {
        return Physics.Raycast(ray, out hit, range, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
    }
}
