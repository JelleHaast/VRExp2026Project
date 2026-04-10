using JetBrains.Annotations;
using NUnit.Framework;
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

    Rigidbody rb;

    public Transform[] hiderSpawnPoints;


    float previousDistance;
    float currentDistance;

    int seekerLocation;

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


        int hiderLocation = Random.Range(0, hiderSpawnPoints.Length);
        int seekerLocation;

        do
        {
            seekerLocation = Random.Range(0, hiderSpawnPoints.Length);
        } while (hiderLocation == seekerLocation);

        rb.position = hiderSpawnPoints[seekerLocation].position;
        hider.position = hiderSpawnPoints[hiderLocation].position;

        previousDistance = Vector3.Distance(transform.position, hider.position);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rb.linearVelocity / speedMultiplier);  // normalized velocity
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(-0.001f);

        // Calculate current distance
        currentDistance = Vector3.Distance(transform.position, hider.position);

        // Reward moving closer
        AddReward((previousDistance - currentDistance) * 0.05f); //(0.05 traing default)

        // Update for next step
        previousDistance = currentDistance;

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


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hider"))
        {
            AddReward(10f);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.5f);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-0.1f);  // shelves, slightly softer than walls (0.05 training default)
        }
    }
}