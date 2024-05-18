using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject resourcePrefab; 
    [SerializeField] private float spawnIntervalMin = 3f; 

    [SerializeField] private Vector3 areaCenter; 
    [SerializeField] private Vector3 areaScale; 

    private List<GameObject> activeResources = new List<GameObject>(); 

    void Start()
    {
        StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnIntervalMin);

            Vector3 spawnPosition = new Vector3(
                Random.Range(areaCenter.x - areaScale.x*2 , areaCenter.x + areaScale.x*2 ),
                areaCenter.y, 
                Random.Range(areaCenter.z - areaScale.z , areaCenter.z + areaScale.z)
            );

            GameObject newResource = Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);
            activeResources.Add(newResource);
        }
    }

    public List<GameObject> GetActiveResources()
    {
        activeResources.RemoveAll(item => item == null); 
        return new List<GameObject>(activeResources); 
    }
}
