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

    private int currentWaypointIndex;
    private Animator animator;

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

            // 💤 Idle
            animator.SetBool("isWalking", false);

            // ✅ Lock X and Z rotation during idle
            Vector3 rot = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rot.y, 0f);

            float waitTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(waitTime);

            PickNewWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        Vector3 targetDir = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        Vector3 finalDirection = targetDir;

        // 🚧 Obstacle detection
        if (IsObstacleAhead(out RaycastHit hit))
        {
            // 👉 Get a side direction to avoid obstacle
            Vector3 avoidDir = Vector3.Cross(hit.normal, Vector3.up).normalized;

            // Mix original + avoid direction
            finalDirection = (targetDir + avoidDir * 2f).normalized;

            // Optional: also change waypoint sometimes
            if (Random.value < 0.02f)
                PickNewWaypoint();
        }

        // Smooth rotation
        if (finalDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(finalDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Always move forward ✅
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void PickNewWaypoint()
    {
        currentWaypointIndex = Random.Range(0, waypoints.Length);
    }

    bool IsObstacleAhead(out RaycastHit hit)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);

        // ✅ Detect EVERYTHING (trigger + non-trigger)
        if (Physics.Raycast(ray, out hit, detectionDistance, ~0, QueryTriggerInteraction.Collide))
        {
            // Ignore ground layer (recommended instead of tag)
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
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