using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using UnityEngine.AI;
public class TimeManager : MonoBehaviour
{
    public Slider TimeSlider;
    private bool slowDown = false;
    private bool speedUp = false;
    private bool normalTime=false;
    public GameManager GameManager;
    public GameObject ImageGo;
    public BuildManager BuildManager;
  

    public float blinkSpeed = 1.0f; 
    public Image alertImage;
    private float targetAlpha = 0.0f;
    private float currentAlpha = 0.0f;


    void Start()
    {
        
        NormalTimeButton();
        TimeSlider.value = 10;
    }

    
    void Update()
    {
        if(slowDown)
        {
            TimeSlider.value -= Time.deltaTime;
            
            foreach (Enemy obj in GameManager.activeEnemies)
            {
                
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    rb.velocity *= 0f;
                    rb.angularVelocity *= 0f;

                }
                
            }
            foreach (GameObject obj in BuildManager.rovers)
            {
                // E�er nesnenin �zerinde NavMeshAgent bile�eni yoksa i�lem yapmay�n
                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    // NavMeshAgent h�z�n� de�i�tirin
                    agent.speed = 0f; // Yeni h�z de�eri
                }
            }
            if (TimeSlider.value == 0)
            {
                Debug.Log("0");
                ImageGo.SetActive(true);
                Alert();
            }
            else
            {
                ImageGo.SetActive(false);
            }

        }
        else if(normalTime)
        {
            TimeSlider.value += Time.deltaTime;
            foreach (Enemy obj in GameManager.activeEnemies)
            {

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                   
                    rb.constraints = RigidbodyConstraints.None;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;


                }
            }
            foreach (GameObject obj in BuildManager.rovers)
            {
                // E�er nesnenin �zerinde NavMeshAgent bile�eni yoksa i�lem yapmay�n
                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    // NavMeshAgent h�z�n� de�i�tirin
                    agent.speed = 3f; // Yeni h�z de�eri
                }
            }
            if (TimeSlider.value == 44)
            {
                Debug.Log("44");
                ImageGo.SetActive(true);
                Alert();
            }
            else
            {
                ImageGo.SetActive(false);
            }

        }

        
    }

    public void Alert()
    {
        
            if (currentAlpha <= 0.0f)
            {
                targetAlpha = 1.0f;
            }
            else if (currentAlpha >= 1.0f)
            {
                targetAlpha = 0.0f;
            }

           
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, blinkSpeed * Time.deltaTime);

           
            Color newColor = alertImage.color;
            newColor.a = currentAlpha;
            alertImage.color = newColor;
        
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
