using JetBrains.Annotations;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class SeekerAgent : Agent
{

    public Transform hider;

    public float rotationMultiplier = 2f;

    public float speedMultiplier = 5f;

    float previousDistance;
    float distanceToHider;

    Rigidbody rb;

    public Transform[] roomCenters; //multiple room spanws

    void Start() { rb = GetComponent<Rigidbody>(); }
    public override void OnEpisodeBegin()
    {
        // Pick random rooms, make sure seeker and target aren't in same room
        int seekerRoom = Random.Range(0, 4);

        int hiderRoom = Random.Range(0, 4);

        while (hiderRoom == seekerRoom)
            hiderRoom = Random.Range(0, 4);

        transform.position = GetRandomSpawnInRoom(roomCenters[seekerRoom]);
        hider.position = GetRandomSpawnInRoom(roomCenters[hiderRoom]);

        previousDistance = Vector3.Distance(transform.position, hider.position);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 relativePos = hider.position - transform.position;
        float distance = relativePos.magnitude;

        sensor.AddObservation(relativePos.normalized);
        sensor.AddObservation(distance / 50f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 1. Tiny time penalty to keep him moving
        AddReward(-0.0005f);

        distanceToHider = Vector3.Distance(transform.position, hider.position);
        float delta = previousDistance - distanceToHider;
        AddReward(delta * 0.0005f); // Positive when closer, negative when farther
        previousDistance = distanceToHider; // Always update

        // 3. Movement (Keep as is)
        float rotation = actionBuffers.ContinuousActions[0];
        float forward = actionBuffers.ContinuousActions[1];
        transform.Rotate(0f, rotation * rotationMultiplier, 0f);
        rb.linearVelocity = transform.forward * forward * speedMultiplier;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    Vector3 GetRandomSpawnInRoom(Transform roomCenter)
    {
        return new Vector3(
            roomCenter.position.x + Random.Range(-3f, 3f),
            0.5f,
            roomCenter.position.z + Random.Range(-3f, 3f)
        );
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hider"))
        {
            AddReward(5f);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.05f);
        }
    }
}
