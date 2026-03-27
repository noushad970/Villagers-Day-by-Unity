using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player
    public Vector3 offset = new Vector3(0, 3, -5);

    public float mouseSensitivity = 3f;
    public float touchSensitivity = 0.2f;
    public float smoothSpeed = 10f;

    float yaw = 0f;
    float pitch = 20f;

    void Start()
    {
        if (target == null)
            Debug.LogError("Camera target not assigned!");
    }

    void LateUpdate()
    {
        HandleRotation();
        FollowTarget();
    }
    public float sideOffset = 1.5f; // adjust this
    void HandleRotation()
    {
        // ===== PC MOUSE (ONLY WHEN LOCKED) =====
        if (Cursor.lockState == CursorLockMode.Locked && Mouse.current != null)
        {
            float mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity * 100f * Time.deltaTime;
            float mouseY = Mouse.current.delta.ReadValue().y * mouseSensitivity * 100f * Time.deltaTime;

            yaw += mouseX;
            pitch -= mouseY;
        }

        // ===== MOBILE TOUCH =====
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == UnityEngine.TouchPhase.Moved)
            {
                yaw += touch.deltaPosition.x * touchSensitivity;
                pitch -= touch.deltaPosition.y * touchSensitivity;
            }
        }

        pitch = Mathf.Clamp(pitch, -30f, 60f);
    }

    void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Camera position
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // 👉 Shift look target to the RIGHT (so player appears LEFT)
        Vector3 lookOffset = target.right * sideOffset;

        Vector3 lookPoint = target.position + Vector3.up * 1.5f + lookOffset;

        transform.LookAt(lookPoint);
    }
}