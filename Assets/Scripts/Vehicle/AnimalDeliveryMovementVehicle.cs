using UnityEngine;

public class AnimalDeliveryMovementVehicle : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private GameObject[] points;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float reachDistance = 0.1f;

    [Header("Logic")]
    public bool canMove = false;

    private int currentPointIndex = 0;
    private bool isVisiting = false;
    private void Start()
    {
        canMove = true;
    }
    void Update()
    {
        if (points == null || points.Length == 0) return;

        // Start a new visit
        if (canMove && !isVisiting)
        {
            StartNewVisit();
        }

        // Continue visit even if canMove becomes false
        if (isVisiting)
        {
            MoveToPoint();
        }
    }

    void StartNewVisit()
    {
        currentPointIndex = 0;
        isVisiting = true;
    }

    void MoveToPoint()
    {
        Transform target = points[currentPointIndex].transform;

        // Move
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Rotate toward target
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Reach check
        if (Vector3.Distance(transform.position, target.position) <= reachDistance)
        {
            currentPointIndex++;

            // Finished all waypoints
            if (currentPointIndex >= points.Length)
            {
                isVisiting = false; // stop only after finishing visit
            }
        }
    }
}
