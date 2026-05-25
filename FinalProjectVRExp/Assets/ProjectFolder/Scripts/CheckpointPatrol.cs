using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class CheckpointPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] checkpoints;
    public float reachRadius = 1.5f;
    public float waitTime = 20.0f;

    [Header("ML Agent")]
    public SeekerNavAgent seekerAgent;

    [Header("Detection Settings")]
    public string targetTag = "Hider";
    public float maxDetectionDistance = 15f;
    [Range(0, 360)] public float fieldOfViewAngle = 180f;
    public LayerMask obstacleMask;

    [Header("Movement Settings")]
    public float normalSpeed = 3.5f;
    public float chaseSpeed = 5.0f;

    private NavMeshAgent nav;
    private Transform playerTransform;
    private int currentIndex = 0;
    private bool isWaiting = false;
    private bool isChasing = false;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = normalSpeed;

        seekerAgent.enabled = false;
        nav.enabled = true;

        GameObject player = GameObject.FindWithTag(targetTag);
        if (player != null)
            playerTransform = player.transform;

        if (checkpoints.Length > 0)
            nav.SetDestination(checkpoints[currentIndex].position);
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (CanSeePlayer() && nav.enabled)
        {
            StartChasing();
            return;
        }

        // Lost sight of player return to patrol
        if (isChasing)
        {
            StopChasing();
            return;
        }

        // Normal patrol logic
        if (!isWaiting && nav.enabled && !nav.pathPending && nav.remainingDistance < reachRadius)
        {
            StartCoroutine(PatrolRoutine());
        }
    }

    bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > maxDetectionDistance) return false;

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer <= fieldOfViewAngle / 2f)
        {
            Vector3 rayStart = transform.position + Vector3.up * 1f;
            Vector3 rayDirection = (playerTransform.position + Vector3.up * 1f) - rayStart;

            if (!Physics.Raycast(rayStart, rayDirection, distanceToPlayer, obstacleMask))
                return true;
        }

        return false;
    }

    void StartChasing()
    {
        if (!isChasing)
        {
            isChasing = true;
            isWaiting = false;
            StopAllCoroutines(); // Cancel any active wait at a checkpoint
            nav.speed = chaseSpeed;
            Debug.Log("Player spotted! Chasing.");
        }

        // Keep updating destination every frame while chasing
        nav.SetDestination(playerTransform.position);
    }

    void StopChasing()
    {
        isChasing = false;
        nav.speed = normalSpeed;

        // Resume patrol from the nearest next checkpoint
        nav.SetDestination(checkpoints[currentIndex].position);
        Debug.Log("Lost sight of player, resuming patrol.");
    }

    IEnumerator PatrolRoutine()
    {
        isWaiting = true;

        nav.isStopped = true;
        nav.enabled = false;
        seekerAgent.enabled = true;
        Debug.Log("Reached checkpoint: ML Agent scanning for " + waitTime + "s");

        yield return new WaitForSeconds(waitTime);

        seekerAgent.enabled = false;
        nav.enabled = true;
        nav.isStopped = false;
        nav.speed = normalSpeed;

        currentIndex = (currentIndex + 1) % checkpoints.Length;
        nav.SetDestination(checkpoints[currentIndex].position);

        Debug.Log("Scan complete: resuming patrol to next checkpoint");
        isWaiting = false;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDetectionDistance);

        Vector3 leftBoundary = Quaternion.AngleAxis(-fieldOfViewAngle / 2f, Vector3.up) * transform.forward;
        Vector3 rightBoundary = Quaternion.AngleAxis(fieldOfViewAngle / 2f, Vector3.up) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftBoundary * maxDetectionDistance);
        Gizmos.DrawRay(transform.position, rightBoundary * maxDetectionDistance);
    }
#endif
}