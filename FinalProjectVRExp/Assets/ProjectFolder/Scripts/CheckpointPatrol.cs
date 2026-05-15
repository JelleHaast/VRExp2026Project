using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CheckpointPatrol : MonoBehaviour
{
    public Transform[] checkpoints;
    public float reachRadius = 1.5f;
    public float waitTime = 20.0f;

    public SeekerNavAgent seekerAgent; // Your ML Agent script

    private NavMeshAgent nav;
    private int currentIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        // Ensure we start in "Travel Mode"
        seekerAgent.enabled = false;
        nav.enabled = true;

        if (checkpoints.Length > 0)
        {
            nav.SetDestination(checkpoints[currentIndex].position);
        }
    }

    void Update()
    {
        // Check if we reached the point and aren't already waiting
        if (!isWaiting && !nav.pathPending && nav.remainingDistance < reachRadius)
        {
            StartCoroutine(PatrolRoutine());
        }
    }

    IEnumerator PatrolRoutine()
    {
        isWaiting = true;

        // --- SWITCH TO SEEKER AGENT ---
        nav.isStopped = true;       // Stop the movement
        nav.enabled = false;
        seekerAgent.enabled = true;  // Enable your ML script
        Debug.Log("Reached Point: ML Agent taking over for " + waitTime + "s");

        // Wait for the 20 seconds
        yield return new WaitForSeconds(waitTime);

        // --- SWITCH BACK TO NAV MESH ---
        seekerAgent.enabled = false; // Disable ML script
        nav.isStopped = false;      // Allow NavMesh to move again
        nav.enabled = true;

        // Pick the next destination
        currentIndex = (currentIndex + 1) % checkpoints.Length;
        nav.SetDestination(checkpoints[currentIndex].position);

        Debug.Log("Wait over: NavMesh resuming to next point");
        isWaiting = false;
    }

    public void OnPlayerDetected()
    {
        StopAllCoroutines();
        nav.enabled = false;
        seekerAgent.enabled = true;
        this.enabled = false; // Disable this patrol script entirely
    }
}