using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class Enemy : Agent
{

    public int hitPoints = 3;
    public bool isDead = false;
    public GameObject particleEffect;
    public GameObject deadPrefab;
    public Image healthImage;


    private Rigidbody rbody;
    public Transform target;
    public Transform body;
    float carpan = 20f;

    public GameManager gm;

    void Start()
    {

        rbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {

        rbody.velocity = rbody.velocity.normalized * 1.5f;
    }


    private void Update()
    {
        Vector3 targetPosition = target.position;

        Vector3 childPosition = body.position;

        body.LookAt(targetPosition);
    }


    public override void OnEpisodeBegin()
    {
        if (transform.localPosition.y < 0)
        {
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0f, 0.5f, 0f);
            transform.rotation = Quaternion.identity;
        }

    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);

        sensor.AddObservation(rbody.velocity.x);
        sensor.AddObservation(rbody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rbody.AddForce(controlSignal * carpan);

        float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);
        if (distanceToTarget < 0.7f)
        {
            AttackToBase();
        }

        if (transform.localPosition.y < 0f)
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }



    void AttackToBase()
    {
        GetHit(100);
        gm.AttackBase();
    }

    public void GetHit(int hitPoint)
    {
        if (hitPoints > 0)
        {
            hitPoints -= hitPoint;
            GameObject hitEffect = Instantiate(particleEffect, this.transform);

            hitEffect.transform.localPosition = new Vector3(0, .5f, 0); // Parent'ýn konumuna göre ayarla
            hitEffect.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            Destroy(hitEffect, 1f);
            ChangeUI();
            if (hitPoints <= 0)
            {
                Die();
            }
        }
        
    }

    void ChangeUI()
    {
        if(hitPoints == 2)
        {
            healthImage.rectTransform.sizeDelta = new Vector2(.66f, 0.2f);
        }
        else if (hitPoints == 1)
        {
            healthImage.rectTransform.sizeDelta = new Vector2(.33f, 0.2f);
        }
    }

    void Die()
    {
        isDead = true;
        Instantiate(deadPrefab, new Vector3(transform.position.x,.5f,transform.position.z),transform.rotation);
        gm.activeEnemies.Remove(transform.GetComponent<Enemy>());
        Destroy(gameObject);
    }
}
