using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject resourcePrefab; 
    [SerializeField] private float spawnIntervalMin = 3f; 

    [SerializeField] private Vector3 areaCenter; 
    [SerializeField] private Vector3 areaScale; 
    [SerializeField] private Vector3 exclusionAreaCenter; 
    [SerializeField] private Vector3 exclusionAreaScale;

    public int counter=0;
    public int spawnNum = 10;

    public List<GameObject> activeResources = new List<GameObject>(); 

    void Start()
    {
        
        StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnResources()
    {
        while (counter < spawnNum)
        {
            if (activeResources.Count < 10 )
            {
                yield return new WaitForSeconds(spawnIntervalMin);

                Vector3 spawnPosition;
                bool isInsideExclusionArea;

                do
                {
                    spawnPosition = new Vector3(
                        Random.Range(areaCenter.x - areaScale.x / 2, areaCenter.x + areaScale.x / 2),
                        areaCenter.y,
                        Random.Range(areaCenter.z - areaScale.z / 2, areaCenter.z + areaScale.z / 2)
                    );

                    isInsideExclusionArea = (spawnPosition.x > exclusionAreaCenter.x - exclusionAreaScale.x / 2 &&
                                             spawnPosition.x < exclusionAreaCenter.x + exclusionAreaScale.x / 2 &&
                                             spawnPosition.z > exclusionAreaCenter.z - exclusionAreaScale.z / 2 &&
                                             spawnPosition.z < exclusionAreaCenter.z + exclusionAreaScale.z / 2);

                } while (isInsideExclusionArea);

                GameObject newResource = Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);
                activeResources.Add(newResource);

                counter++;
            }
            else
            {
                yield return new WaitForSeconds((int)spawnIntervalMin);
            }
            
        }
    }

    public List<GameObject> GetActiveResources()
    {
        return activeResources;
    }
}
