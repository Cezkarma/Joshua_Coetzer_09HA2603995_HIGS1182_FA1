using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float minSpeed = 14f;
    [SerializeField] private float maxSpeed = 24f;

    [Header("Lifetime")]
    [Tooltip("Seconds the asteroid stays alive before despawning.")]
    [SerializeField] private float lifetime = 45f;

    public void Launch(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.linearVelocity = direction.normalized * Random.Range(minSpeed, maxSpeed);

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponentInParent<PlayerController>() == null)
        {
            return;
        }

        GameManager.EndRun();
    }
}
