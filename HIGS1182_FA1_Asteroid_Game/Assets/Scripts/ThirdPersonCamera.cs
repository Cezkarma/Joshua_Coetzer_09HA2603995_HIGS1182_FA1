using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
    [Tooltip("Left empty, the first PlayerController in the scene is used.")]
    [SerializeField] private PlayerController target;

    [Header("Framing")]
    [Tooltip("Where the camera sits relative to the ship, in the ship's aim space.")]
    [SerializeField] private Vector3 followOffset = new Vector3(0f, 2.5f, -9f);
    [Tooltip("How far ahead of the ship the camera looks, which keeps the nose low in frame.")]
    [SerializeField] private float lookAheadDistance = 15f;

    [Header("Smoothing")]
    [Tooltip("Seconds the camera takes to catch up in position. Higher feels heavier.")]
    [SerializeField] private float positionSmoothTime = 0.08f;
    [Tooltip("How sharply the camera swings onto the new aim. Higher is snappier.")]
    [SerializeField] private float rotationSharpness = 20f;

    [Header("Speed Effects")]
    [SerializeField] private bool speedAffectsFov = true;
    [Tooltip("Degrees added to the camera's starting field of view at full speed.")]
    [SerializeField] private float maxFovBoost = 12f;
    [SerializeField] private float fovSharpness = 4f;

    private Camera cam;
    private float baseFov;
    private Vector3 positionVelocity;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        baseFov = cam.fieldOfView;

        if (target == null)
        {
            target = FindFirstObjectByType<PlayerController>();
        }
    }

    private void Start()
    {
        SnapToTarget();
    }

    public void SetTarget(PlayerController newTarget)
    {
        target = newTarget;
        SnapToTarget();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Quaternion aim = target.AimRotation;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            GetDesiredPosition(aim),
            ref positionVelocity,
            positionSmoothTime);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            GetDesiredRotation(aim, transform.position),
            1f - Mathf.Exp(-rotationSharpness * Time.deltaTime));

        if (speedAffectsFov)
        {
            float targetFov = baseFov + maxFovBoost * target.SpeedFraction;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 1f - Mathf.Exp(-fovSharpness * Time.deltaTime));
        }
    }

    private void SnapToTarget()
    {
        if (target == null)
        {
            return;
        }

        Quaternion aim = target.AimRotation;
        positionVelocity = Vector3.zero;

        transform.position = GetDesiredPosition(aim);
        transform.rotation = GetDesiredRotation(aim, transform.position);
    }

    private Vector3 GetDesiredPosition(Quaternion aim)
    {
        return target.transform.position + aim * followOffset;
    }

    private Quaternion GetDesiredRotation(Quaternion aim, Vector3 fromPosition)
    {
        Vector3 lookPoint = target.transform.position + aim * Vector3.forward * lookAheadDistance;
        Vector3 toLookPoint = lookPoint - fromPosition;

        if (toLookPoint.sqrMagnitude < 0.0001f)
        {
            return aim;
        }

        return Quaternion.LookRotation(toLookPoint, aim * Vector3.up);
    }
}
