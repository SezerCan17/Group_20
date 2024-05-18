using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Market Item", menuName = "Market Data", order = 1)]

public class MarketItem : ScriptableObject
{
    public string itemName;
    public string type;
    public int copper_cost;
    public int iron_cost;
    public int gold_cost;
    public int diamond_cost;
    public string description;
    public Image icon;

    public GameObject inGameModel;
    public GameObject placementModel;
}
