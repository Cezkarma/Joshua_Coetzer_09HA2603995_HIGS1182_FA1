using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        if (movement.magnitude > 0f)
        {
            movement.Normalize();

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            transform.rotation = Quaternion.LookRotation(movement) * Quaternion.Euler(-90f, 0f, 0f);
        }
    }
}