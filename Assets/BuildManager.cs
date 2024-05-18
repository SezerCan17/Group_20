using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class BuildManager : MonoBehaviour
{


    public PlayerController player;
    public GameObject marketPlace;
    public GameObject neRPanel;


    public ResourceSpawner blueResourceSpawn;
    public ResourceSpawner yellowResourceSpawn;
    public ResourceSpawner greenResourceSpawn;
    public ResourceSpawner redResourceSpawn;

    public GameObject resourceDrop;

    private GameObject currentPlacementModel;
    private MarketItem currentItem;

    public AudioClip buildingClip;

    public MarketItem[] items;

    public Material placeMaterial;

    public LayerMask hitMask;

    bool canPlace = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlacementModel != null)
        {
            FollowMouse();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceItem();
            }
        }
    }


    public void OpenMarket()
    {
        marketPlace.SetActive(true);
        player.buildMode = true;
    }

    public void CloseMarket()
    {
        marketPlace.SetActive(false);
        player.buildMode = false;
    }


    public void BuildItem(string itemName)
    {
        foreach (MarketItem item in items)
        {
            if (item.itemName == itemName)
            {

                currentItem = item;
                // if(player.copper > item.copper_cost && player.iron > item.iron_cost && player.gold > item.gold_cost && player.diamond > item.diamond_cost)
                // {
                if (item.type == "rover")
                {
                    if (item.inGameModel != null)
                    {
                        DrawResource(item);
                        GameObject spawnedRover = Instantiate(item.inGameModel, Vector3.zero, Quaternion.identity);
                        spawnedRover.GetComponent<Rover>().depotTransform = resourceDrop.transform;
                        Debug.Log(item.itemName);
                        if(item.itemName == "green_rover")
                        {
                            spawnedRover.GetComponent<Rover>().spawner = greenResourceSpawn;
                        }
                        else if(item.itemName == "yellow_rover")
                        {
                            spawnedRover.GetComponent<Rover>().spawner = yellowResourceSpawn;
                        }
                        else if (item.itemName == "blue_rover")
                        {
                            spawnedRover.GetComponent<Rover>().spawner = blueResourceSpawn;
                        }
                        else if (item.itemName == "red_rover")
                        {
                            spawnedRover.GetComponent<Rover>().spawner = redResourceSpawn;
                        }
                        
                        player.audioSource.PlayOneShot(buildingClip);

                    }
                }
                else if(item.type == "turret")
                {
                    currentPlacementModel = Instantiate(item.placementModel, Vector3.zero, Quaternion.identity);
                    marketPlace.SetActive(false);
                }
                //  }
                // else
                // {
                //        marketPlace.SetActive(false);
                //        neRPanel.SetActive(true);
                //        player.buildMode = false;
                // }



                return; 
            }
        }
    }

    void DrawResource(MarketItem item)
    {
        // player.copper -= item.copper_cost
        // player.iron -= item.iron_cost
        // player.gold -= item.gold_cost
        // player.diamond -= item.diamond_cost
    }
    void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, hitMask))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = 0; 
            currentPlacementModel.transform.position = targetPosition;

            Debug.Log(hit.transform.tag);
            if(hit.transform.CompareTag("Base"))
            {
                placeMaterial.color = Color.red;
                canPlace = false;
            }
            else
            {
                placeMaterial.color = Color.green;
                canPlace = true;
            }
        }
    }


    void PlaceItem()
    {
        if (currentItem != null && currentItem.inGameModel != null && canPlace)
        {
            Instantiate(currentItem.inGameModel, currentPlacementModel.transform.position, Quaternion.identity);
            Destroy(currentPlacementModel);
            DrawResource(currentItem);
            currentItem = null;
            player.audioSource.PlayOneShot(buildingClip);
            player.shootTimer = 1.2f;
            player.buildMode = false;
            canPlace = false;
        }
    }
}
