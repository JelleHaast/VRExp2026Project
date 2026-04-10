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

    public Transform seekerStart;

    float previousDistance;
    float currentDistance;

    int seekerLocation;
    float wallContactTime = 0f;

    //public Transform[] seekerLocations; //multiple room spanws

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;
        rb.angularDamping = 1f;
    }
    public override void OnEpisodeBegin()
    {

        transform.position = seekerStart.position;
        transform.rotation = seekerStart.rotation;
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
        AddReward(-0.0005f);

        // Calculate current distance
        currentDistance = Vector3.Distance(transform.position, hider.position);

        // Reward moving closer
        AddReward((previousDistance - currentDistance) * 0.005f); //(0.05 traing default)

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


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("wallcontacttime" + wallContactTime);
            wallContactTime += Time.fixedDeltaTime;
            AddReward(-0.002f * wallContactTime);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
            wallContactTime = 0f;
    }
}
