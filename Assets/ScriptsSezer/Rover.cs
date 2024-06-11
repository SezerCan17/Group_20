using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEditor;
public class Rover : MonoBehaviour
{
    [SerializeField] public ResourceSpawner spawner; 
    [SerializeField] private TMP_Text resourceCounterText; 
    [SerializeField] public Transform depotTransform;
    [SerializeField] private string gemrengi;

    public ResourceController ResourceController { get; private set; }

    private NavMeshAgent _agent;
    private GameObject currentDestination;
    public int resourceCount = 0; 
    public int carriedResourceCount = 0;
    public AudioSource collectResource;

    private void Awake()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
    }
    void Start()
    {
        ResourceController = FindObjectOfType<ResourceController>();
        _agent = GetComponent<NavMeshAgent>();
        FindNextDestination();
        UpdateResourceCounterText(); 
    }

    void Update()
    {
        if (currentDestination != null)
        {
            _agent.SetDestination(currentDestination.transform.position);

            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                CollectCurrentDestination();
            }
        }
        else
        {
            if (carriedResourceCount >= 5)
            {
                GoToDepot();
            }
            else
            {
                FindNextDestination();
            }
        }
    }

    void FindNextDestination()
    {
        List<GameObject> activeResources = spawner.GetActiveResources();

        if (activeResources.Count == 0)
        {
            currentDestination = null;
            StartCoroutine(WaitAndFind());
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

    IEnumerator WaitAndFind()
    {
        yield return new WaitForSeconds(5f);
        FindNextDestination();
    }
    void CollectCurrentDestination()
    {
        if (currentDestination != null)
        {
            gemrengi = currentDestination.tag;
            spawner.activeResources.Remove(currentDestination.gameObject);
            Destroy(currentDestination);
            collectResource.Play();
            currentDestination = null;
            carriedResourceCount++; 

            if (carriedResourceCount >= 1)
            {
                GoToDepot();
            }
            else
            {
                FindNextDestination();
            }
        }
    }

    void GoToDepot()
    {
        _agent.SetDestination(depotTransform.position);
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            DepositResources();
            FindNextDestination();
        }
    }

    void DepositResources()
    {
        UpdateResourceCounterText(); 
        carriedResourceCount = 0; 
        
    }


    void UpdateResourceCounterText()
    {
       
            switch(gemrengi)
            {
                case "Bluegem":
                    Debug.Log("Geldi mavi");
                    ResourceController.BlueGemManager(carriedResourceCount, true);
                    break;
                case "Redgem":
                    Debug.Log("Geldi red");
                    ResourceController.RedGemManager(carriedResourceCount, true);
                    break;
                case "Greengem":
                    Debug.Log("Geldi yeþil");
                    ResourceController.GreenGemManager(carriedResourceCount, true);
                    break;
                case "Yellowgem":
                    Debug.Log("Geldi sarý");
                    ResourceController.YellowGemManager(carriedResourceCount, true);
                    break;

            }
        }
    
}
