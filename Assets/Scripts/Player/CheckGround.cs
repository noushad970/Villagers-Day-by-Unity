using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Settings")]
    public Transform groundCheck;      // Foot position
    public float checkRadius = 0.2f;   // Small sphere radius
    public LayerMask groundLayer;      // Ground layer mask

    [HideInInspector]
    public static bool isGrounded;

    void Update()
    {
        // Check if sphere at foot touches ground
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);

        // Debug
        if (isGrounded)
            Debug.Log("Player is Grounded");
        else
            Debug.Log("Player is NOT Grounded");
    }

    // Optional: visualize in scene
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
