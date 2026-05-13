using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SeekerNavAgent : Agent
{
    public float rotationMultiplier = 2f;
    public float speedMultiplier = 5f;

    private Rigidbody rb;

    public override void Initialize()
    {
        Debug.Log("AI Initialize started on " + gameObject.name);
        if (rb == null) Debug.LogError("RB IS MISSING ON " + gameObject.name);
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearDamping = 1f;
            rb.angularDamping = 1f;
        }
    }

    public override void OnEpisodeBegin()
    {
        if (rb == null) return; // Safety guard

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (rb == null)
        {
            sensor.AddObservation(Vector3.zero);
            return;
        }
        sensor.AddObservation(rb.linearVelocity / speedMultiplier);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (rb == null) return;

        float rotation = actionBuffers.ContinuousActions[0];
        float forward = actionBuffers.ContinuousActions[1];

        // Apply Rotation
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation * rotationMultiplier, 0f));

        // Apply Movement
        Vector3 move = transform.forward * forward * speedMultiplier * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
        continuousActionsOut[1] = Input.GetAxisRaw("Vertical");
    }
}