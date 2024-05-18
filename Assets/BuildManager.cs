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

    private GameObject currentPlacementModel;
    private MarketItem currentItem;

    public AudioClip buildingClip;

    public MarketItem[] items;
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
                        Instantiate(item.inGameModel, Vector3.zero, Quaternion.identity);
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
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = 0; 
            currentPlacementModel.transform.position = targetPosition;
        }
    }


    void PlaceItem()
    {
        if (currentItem != null && currentItem.inGameModel != null)
        {
            Instantiate(currentItem.inGameModel, currentPlacementModel.transform.position, Quaternion.identity);
            Destroy(currentPlacementModel);
            DrawResource(currentItem);
            currentItem = null;
            player.audioSource.PlayOneShot(buildingClip);
            player.shootTimer = 1.2f;
            player.buildMode = false;
        }
    }
}
