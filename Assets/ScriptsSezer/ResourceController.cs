using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceController : MonoBehaviour
{
    [SerializeField]
    public int copper_count=0;
    [SerializeField]
    public int iron_count = 0;
    [SerializeField]
    public int gold_count = 0;
    [SerializeField]
    public int diamond_count = 0;

    public TMP_Text blueGemText;
    public TMP_Text redGemText;
    public TMP_Text greenGemText;
    public TMP_Text yellowGemText;


    public void BlueGemManager(int blueGemNumber, bool plus)
    {
        if(plus)
        {
            diamond_count += blueGemNumber;
            blueGemText.text = "Elmas: " + diamond_count;
        }
        else
        {
            diamond_count -= blueGemNumber;
            blueGemText.text = "Elmas: " + diamond_count;
        }
        
    }

    public void RedGemManager(int redGemNumber, bool plus)
    {
        if (plus)
        {

            copper_count += redGemNumber;
            redGemText.text = "Bak�r: " + copper_count;
        }
        else
        {
            copper_count -= redGemNumber;
            redGemText.text = "Bak�r: " + copper_count;
        }

    }

    public void GreenGemManager(int greenGemNumber, bool plus)
    {
        if (plus)
        {
            iron_count += greenGemNumber;
            greenGemText.text = "Demir: " + iron_count;
        }
        else
        {
            iron_count -= greenGemNumber;
            greenGemText.text = "Demir: " + iron_count;
        }

    }

    public void YellowGemManager(int yellowGemNumber, bool plus)
    {
        if (plus)
        {
            gold_count += yellowGemNumber;
            yellowGemText.text = "Alt�n: " + gold_count;
        }
        else
        {
            gold_count -= yellowGemNumber;
            yellowGemText.text = "Alt�n: " + gold_count;
        }

    }




}
