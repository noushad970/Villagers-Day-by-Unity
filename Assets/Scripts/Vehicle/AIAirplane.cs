using UnityEngine;

public class AIAirplane : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Speed Settings")]
    public float maxSpeed = 30f;
    public float acceleration = 5f;
    public float deceleration = 8f;
    public float rotationSpeed = 2f;
    public float reachDistance = 5f;

    [Header("Final Idle Rotation")]
    public Vector3 finalRotationEuler = new Vector3(0, 180, 0);
    public float finalRotationSpeed = 2f;

    private int currentWaypointIndex = 0;
    private float currentSpeed = 0f;
    private bool isFlying = false;
    private bool isStoppingPhase = false;
    private bool isReturningToIdleRotation = false;

    private void Start()
    {
        StartFly();
    }
    void Update()
    {
        if (!isFlying || waypoints.Length == 0)
            return;

        if (isReturningToIdleRotation)
        {
            RotateToFinalDirection();
            return;
        }

        MovePlane();
    }

    // ===============================
    // PUBLIC FUNCTIONS
    // ===============================

    public void StartFly()
    {
        if (isFlying) return;
        if (waypoints.Length < 2) return;

        currentWaypointIndex = 0;
        currentSpeed = 0f;
        isFlying = true;
        isStoppingPhase = false;
        AudioManager.Instance.StartVehicleEnginePlane();
        isReturningToIdleRotation = false;
    }

    public void StopFly()
    {
        isFlying = false;
        currentSpeed = 0f;
        AudioManager.Instance.StopVehicleEnginePlane();
    }

    // ===============================
    // FLIGHT LOGIC
    // ===============================
    public bool getFlyingStatus()
    {
        return isFlying;
    }
    void MovePlane()
    {
        Transform target = waypoints[currentWaypointIndex];

        Vector3 direction = (target.position - transform.position).normalized;

        // Smooth look toward waypoint
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime
        );

        HandleSpeed();

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < reachDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                // Reached final waypoint
                currentSpeed = 0f;
                isReturningToIdleRotation = true;
            }
        }
    }

    void HandleSpeed()
    {
        if (currentWaypointIndex >= waypoints.Length - 2)
            isStoppingPhase = true;

        if (!isStoppingPhase)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                maxSpeed,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                deceleration * Time.deltaTime
            );
        }
    }

    void RotateToFinalDirection()
    {
        Quaternion targetRotation = Quaternion.Euler(finalRotationEuler);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            finalRotationSpeed * Time.deltaTime
        );

        // Check if rotation is almost equal
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            transform.rotation = targetRotation;
            isReturningToIdleRotation = false;
            StopFly();
        }
    }
}