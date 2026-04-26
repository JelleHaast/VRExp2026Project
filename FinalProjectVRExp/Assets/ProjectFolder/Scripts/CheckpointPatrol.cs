using UnityEngine;
using UnityEngine.AI;

public class CheckpointPatrol : MonoBehaviour
{
    public Transform[] checkpoints;
    public float reachRadius = 1.5f;

    public SeekerNavAgent seekerAgent;

    private NavMeshAgent nav;
    private int currentIndex = 0;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        seekerAgent.enabled = false;
        nav.SetDestination(checkpoints[currentIndex].position);
    }

    void Update()
    {
        if (nav.remainingDistance < reachRadius && !nav.pathPending)
        {
            currentIndex = (currentIndex + 1) % checkpoints.Length;
            nav.SetDestination(checkpoints[currentIndex].position);
        }
    }

    public void OnPlayerDetected()
    {
        enabled = false;
        nav.enabled = false;
        seekerAgent.enabled = true;
    }
}