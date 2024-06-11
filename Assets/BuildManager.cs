using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using Unity.VisualScripting;

public class BuildManager : MonoBehaviour
{

    public GameManager gm;

    public PlayerController player;
    public GameObject marketPlace;
    public GameObject neRPanel;
    public List<GameObject> rovers;


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
                 if(gm.resources.copper_count >= item.copper_cost && gm.resources.iron_count >= item.iron_cost && gm.resources.gold_count >= item.gold_cost && gm.resources.diamond_count >= item.diamond_cost)
                 {
                    if (item.type == "rover")
                    {
                        if (item.inGameModel != null)
                        {
                            DrawResource(item);
                            GameObject spawnedRover = Instantiate(item.inGameModel, Vector3.zero, Quaternion.identity);
                            spawnedRover.GetComponent<Rover>().depotTransform = resourceDrop.transform;
                            if(item.itemName == "green_rover")
                            {
                                spawnedRover.GetComponent<Rover>().spawner = greenResourceSpawn;
                                rovers.Add(spawnedRover);
                                
                            }
                            else if(item.itemName == "yellow_rover")
                            {
                                spawnedRover.GetComponent<Rover>().spawner = yellowResourceSpawn;
                                rovers.Add(spawnedRover);
                            }
                            else if (item.itemName == "blue_rover")
                            {
                                spawnedRover.GetComponent<Rover>().spawner = blueResourceSpawn;
                                rovers.Add(spawnedRover);
                            }
                            else if (item.itemName == "red_rover")
                            {
                                spawnedRover.GetComponent<Rover>().spawner = redResourceSpawn;
                                rovers.Add(spawnedRover);
                            }
                        
                            player.audioSource.PlayOneShot(buildingClip);
                            player.shootTimer = 1.2f;
                            player.buildMode = false;
                            canPlace = false;

                        }
                    }
                    else if(item.type == "turret")
                    {
                        currentPlacementModel = Instantiate(item.placementModel, Vector3.zero, Quaternion.identity);
                        marketPlace.SetActive(false);
                        player.shootTimer = 1.2f;
                        player.buildMode = false;
                        canPlace = false;
                    }
                 }
                else
                {
                        marketPlace.SetActive(false);
                        neRPanel.SetActive(true);
                        player.buildMode = false;
                        canPlace = false;
                }
                //return; 
            }
        }
    }

    void DrawResource(MarketItem item)
    {
        gm.resources.YellowGemManager(item.diamond_cost, false);
        gm.resources.RedGemManager(item.copper_cost, false);
        gm.resources.BlueGemManager(item.gold_cost, false);
        gm.resources.GreenGemManager(item.iron_cost, false);
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
