using System.Collections;
using UnityEngine;

public class VillagerNPC : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    [Header("Idle")]
    public float minIdleTime = 2f;
    public float maxIdleTime = 5f;

    [Header("Obstacle Avoidance")]
    public float detectionDistance = 1.5f;
    public float avoidStrength = 3f;

    private int currentWaypointIndex;
    private Animator animator;

    private Vector3 avoidDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        PickNewWaypoint();
        StartCoroutine(NPCRoutine());
    }

    IEnumerator NPCRoutine()
    {
        while (true)
        {
            animator.SetBool("isWalking", true);

            while (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) > 0.2f)
            {
                MoveToWaypoint();
                yield return null;
            }

            // Idle
            animator.SetBool("isWalking", false);
            float waitTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(waitTime);

            PickNewWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        Vector3 targetDir = (waypoints[currentWaypointIndex].position - transform.position).normalized;

        // 🚧 Obstacle detection & avoidance
        Vector3 finalDirection = targetDir;

        if (IsObstacleAhead(out RaycastHit hit))
        {
            PickNewWaypoint();
        }

        // Smooth rotation
        if (finalDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(finalDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Move forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void PickNewWaypoint()
    {
        currentWaypointIndex = Random.Range(0, waypoints.Length);
    }

    bool IsObstacleAhead(out RaycastHit hit)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);

        if (Physics.Raycast(ray, out hit, detectionDistance))
        {
            // Ignore ground using tag
            if (hit.collider.CompareTag("Land"))
                return false;

            return true;
        }

        return false;
    }

    // 🔍 Debug rays
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawLine(origin, origin + transform.forward * detectionDistance);
    }
}