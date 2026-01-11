using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCameraController : MonoBehaviour
{
    [Header("Player and Camera")]
    public Transform player;       // assign your player transform
    public Transform cameraPivot;  // empty GameObject above player (usually at head level)

    [Header("Rotation Settings")]
    public float rotationSpeed = 0.2f; // swipe sensitivity
    public float minVerticalAngle = -45f; // lowest look angle
    public float maxVerticalAngle = 60f;  // highest look angle

    private Vector2 lastTouchPosition;
    private bool isTouching = false;

    private float xRotation = 0f; // vertical rotation
    private float yRotation = 0f; // horizontal rotation

    void Start()
    {
        // Initialize rotation based on current camera orientation
        Vector3 angles = cameraPivot.localEulerAngles;
        xRotation = angles.x;
        yRotation = player.eulerAngles.y;
    }

    void Update()
    {
        HandleTouchRotation();
    }

    void HandleTouchRotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                Vector2 delta = touch.position - lastTouchPosition;

                // Horizontal rotation
                yRotation += delta.x * rotationSpeed;

                // Vertical rotation
                xRotation -= delta.y * rotationSpeed; // subtract to invert drag
                xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

                // Apply rotations
                player.rotation = Quaternion.Euler(0f, yRotation, 0f); // rotate player horizontally
                cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // rotate camera vertically

                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
            }
        }
    }
}
