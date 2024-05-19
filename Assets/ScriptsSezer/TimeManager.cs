using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimeManager : MonoBehaviour
{
    public Slider TimeSlider;
    private bool slowDown = false;
    private bool speedUp = false;
    private bool normalTime=false;


    void Start()
    {
        TimeSlider.value = 10;
    }

    
    void Update()
    {
        if(slowDown)
        {
            TimeSlider.value -= Time.deltaTime;
            //Uzaylýlarýn hýzlarýný yavaþlat!!!!
            if(TimeSlider.value==0)
            {
                NormalTimeButton();
            }
        }
        else if(speedUp)
        {
            TimeSlider.value += Time.deltaTime;
            //Uzaylýlarýn hýzlarýný artýr!!!!
            if (TimeSlider.value==20)
            {
                NormalTimeButton();
            }
        }
    }
    public void SlowDownButton()
    {
        slowDown = true;
        speedUp = false;
        normalTime = false;
    }

    public void SpeedUpButton()
    {
        slowDown=false;
        speedUp = true;
        normalTime = false;
    }

    public void NormalTimeButton()
    {
        slowDown = false;
        speedUp = false;
        normalTime = true;
    }


    
}
