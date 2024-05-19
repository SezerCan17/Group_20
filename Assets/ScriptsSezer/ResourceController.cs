using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TMP_Text blueGemText;
    public TMP_Text redGemText;
    public TMP_Text greenGemText;
    public TMP_Text yellowGemText;


    public void BlueGemManager(int blueGemNumber, bool plus)
    {
        if(plus)
        {
            Debug.Log("geldiiii");
            blueGem_Number= blueGemNumber;
            blueGemText.text = "Demir:" + blueGemNumber;
        }
        else
        {
            blueGem_Number -= blueGemNumber;
            blueGemText.text = "Demir:" + blueGemNumber;
        }
        
    }

    public void RedGemManager(int redGemNumber, bool plus)
    {
        if (plus)
        {

            redGem_Number += redGemNumber;
            redGemText.text = "Bakýr:" + redGemNumber;
        }
        else
        {
            redGem_Number -= redGemNumber;
            redGemText.text = "Bakýr:" + redGemNumber;
        }

    }

    public void GreenGemManager(int greenGemNumber, bool plus)
    {
        if (plus)
        {
            greenGem_Number += greenGemNumber;
            greenGemText.text = "Elmas:" + greenGemNumber;
        }
        else
        {
            greenGem_Number -= greenGemNumber;
            greenGemText.text = "Elmas:" + greenGemNumber;
        }

    }

    public void YellowGemManager(int yellowGemNumber, bool plus)
    {
        if (plus)
        {
            yellowGem_Number += yellowGemNumber;
            yellowGemText.text = "Altýn:" + yellowGemNumber;
        }
        else
        {
            yellowGem_Number -= yellowGemNumber;
            yellowGemText.text = "Altýn:" + yellowGemNumber;
        }

    }




}
