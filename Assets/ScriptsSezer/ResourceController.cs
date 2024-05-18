using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField]
    private int blueGem_Number=0;
    [SerializeField]
    private int redGem_Number = 0;
    [SerializeField]
    private int greenGem_Number = 0;
    [SerializeField]
    private int yellowGem_Number = 0;   
    public void BlueGemManager(int blueGemNumber, bool plus)
    {
        if(plus)
        {
            Debug.Log("geldiiii");
            blueGem_Number += blueGemNumber;
        }
        else
        {
            blueGem_Number -= blueGemNumber;
        }
        
    }

    public void RedGemManager(int redGemNumber, bool plus)
    {
        if (plus)
        {

            redGem_Number += redGemNumber;
        }
        else
        {
            redGem_Number -= redGemNumber;
        }

    }

    public void GreenGemManager(int greenGemNumber, bool plus)
    {
        if (plus)
        {
            greenGem_Number += greenGemNumber;
        }
        else
        {
            greenGem_Number -= greenGemNumber;
        }

    }

    public void YellowGemManager(int yellowGemNumber, bool plus)
    {
        if (plus)
        {
            yellowGem_Number += yellowGemNumber;
        }
        else
        {
            yellowGem_Number -= yellowGemNumber;
        }

    }




}
