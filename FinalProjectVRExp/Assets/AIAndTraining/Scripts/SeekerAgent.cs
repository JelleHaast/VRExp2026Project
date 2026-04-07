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

    Rigidbody rb;

    void Start() { rb = GetComponent<Rigidbody>(); }
    public override void OnEpisodeBegin()
    {
        this.transform.position = new Vector3(
            Random.Range(-4f, 4f),
            0.5f,
            Random.Range(-4f, 4f)
        );

        hider.transform.position = new Vector3(
            Random.Range(-4f, 4f),
            0.5f,
            Random.Range(-4f, 4f)
        );

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //not needed for ray perception training
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(-0.0005f);

        // Get Actions
        float rotation = actionBuffers.ContinuousActions[0];
        float forward = actionBuffers.ContinuousActions[1];

        //apply movement
        transform.Rotate(0f, rotation * rotationMultiplier, 0f);
        rb.linearVelocity = transform.forward * forward * speedMultiplier;

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
            AddReward(3f);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.05f);
        }
    }
}
