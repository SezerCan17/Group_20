using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rover : MonoBehaviour
{
    [SerializeField] private ResourceSpawner spawner; // Resource spawner referansý

    private NavMeshAgent _agent;
    private GameObject currentDestination;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        FindNextDestination();
    }

    void Update()
    {
        if (currentDestination != null)
        {
            _agent.SetDestination(currentDestination.transform.position);

            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                CollectCurrentDestination();
                FindNextDestination();
            }
        }
        else
        {
            FindNextDestination();
        }
    }

    void FindNextDestination()
    {
        List<GameObject> activeResources = spawner.GetActiveResources();

        if (activeResources.Count == 0)
        {
            currentDestination = null;
            return;
        }

        float shortestDistance = Mathf.Infinity;
        GameObject nearestPoint = null;

        foreach (var point in activeResources)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPoint = point;
            }
        }

        currentDestination = nearestPoint;
    }

    void CollectCurrentDestination()
    {
        if (currentDestination != null)
        {
            Destroy(currentDestination);
            currentDestination = null;
        }
    }
}
