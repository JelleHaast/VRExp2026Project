using JetBrains.Annotations;
using NUnit.Framework;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class SeekerNavAgent : Agent
{
    public float rotationMultiplier = 2f;

    public float speedMultiplier = 5f;

    Rigidbody rb;

    //public Transform[] seekerLocations; //multiple room spanws

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;
        rb.angularDamping = 1f;
    }
    public override void OnEpisodeBegin()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rb.linearVelocity / speedMultiplier);  // normalized velocity
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float rotation = actionBuffers.ContinuousActions[0];
        float forward = actionBuffers.ContinuousActions[1];

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation * rotationMultiplier, 0f));
        Vector3 move = transform.forward * forward * speedMultiplier * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}