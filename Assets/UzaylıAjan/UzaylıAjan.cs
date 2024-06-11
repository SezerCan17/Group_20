using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEditor;
public class UzaylıAjan : Agent
{
   
    private Rigidbody rbody;
    public Transform target;
    public Transform body;
    public float carpan = 20f;

    // Initialize the agent
    void Start()
    {
        
        rbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

            rbody.velocity = rbody.velocity.normalized * 2.5f;
    }
    private void Update()
    {
        Vector3 targetPosition = target.position;

        // Child pozisyonunu belirle
        Vector3 childPosition = body.position;

        // Hedef pozisyonuna doğru çevir
        body.LookAt(targetPosition);
    }

    // Called when the agent's episode begins
    public override void OnEpisodeBegin()
    {
        // Reset position if the agent fell
        if (transform.localPosition.y < 0)
        {
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0f, 0.5f, 0f);
            transform.rotation = Quaternion.identity;
        }

        // Move the target to a new random position
        target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.5f,
                                           Random.value * 8 - 4);
    }

    // Collect observations for the agent
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rbody.velocity.x);
        sensor.AddObservation(rbody.velocity.z);
    }

    // Process actions received from the agent's policy
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rbody.AddForce(controlSignal * carpan);


        


        // Rewards
        float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);
        if (distanceToTarget < 3f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        if (transform.localPosition.y < 0f)
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    // Provide a heuristic for manual control (for debugging purposes)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
