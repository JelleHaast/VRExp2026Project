using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class CheckpointPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] checkpoints1;
    public Transform[] checkpoints2;

    public QuestData quest;
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
    private Transform[] _activeCheckpoints;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = normalSpeed;

        seekerAgent.enabled = false;
        nav.enabled = true;

        _activeCheckpoints = checkpoints1;

        GameObject player = GameObject.FindWithTag(targetTag);
        if (player != null)
            playerTransform = player.transform;

        if (_activeCheckpoints.Length > 0)
            nav.SetDestination(_activeCheckpoints[currentIndex].position);
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Detect quest completion and switch routes immediately
        if (quest.isCompleted && _activeCheckpoints != checkpoints2)
        {
            _activeCheckpoints = checkpoints2;
            currentIndex = 0;
            isWaiting = false;
            isChasing = false;
            StopAllCoroutines();

            // Re-enable nav in case it was disabled during ML scan
            seekerAgent.enabled = false;
            nav.enabled = true;
            nav.isStopped = false;

            nav.SetDestination(_activeCheckpoints[0].position);
            Debug.Log("Quest completed! Switching to checkpoint route 2.");
            return;
        }

        if (CanSeePlayer() && nav.enabled)
        {
            StartChasing();
            return;
        }

        if (isChasing)
        {
            StopChasing();
            return;
        }

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
            StopAllCoroutines();
            nav.speed = chaseSpeed;
            Debug.Log("Player spotted! Chasing.");
        }

        nav.SetDestination(playerTransform.position);
    }

    void StopChasing()
    {
        isChasing = false;
        nav.speed = normalSpeed;
        nav.SetDestination(_activeCheckpoints[currentIndex].position);
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

        currentIndex = (currentIndex + 1) % _activeCheckpoints.Length;
        nav.SetDestination(_activeCheckpoints[currentIndex].position);

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