using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Google.Protobuf.WellKnownTypes;

public class GameManager : MonoBehaviour
{
    public AutoFlip autoFlip;
    public GameObject MainMenu;
    public GameObject Story1;
    public GameObject Story2;
    public GameObject Story3;
    public ResourceController resources;
    public float counter = 0;
    public Image DayImage;
    public int childCount;
   
  
    public bool start=false;

    public int day;
    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject MarketMenu;
    public bool isPlaying = false;

    int spawnCount = 0;

    float lastSpawnTime = 0;
    float nextSpawnDelay = 0;

    public TextMeshProUGUI dayText;
    public GameObject dayPanel;


    bool timeSlow = false;

    public List<Enemy> activeEnemies;
    public GameObject enemyPrefab;
    public Transform enemyTarget;

    public Transform healthBar;

    private void Awake()
    {
        PauseMenu.SetActive(false);
        PrepareDay();
    }
    private void Update()
    {
        
        

        if(Input.GetKeyDown(KeyCode.T))
        {
            if(timeSlow)
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }

            timeSlow = !timeSlow;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale==1f)
            {
                PauseButton();
            }
            else if(Time.timeScale==0f)
            {
                ResumeButton();
            }
            
        }

        


        if(isPlaying)
        {
            if (spawnCount < (day * 10))
            {
                if(Time.time - lastSpawnTime > nextSpawnDelay)
                {
                    SpawnEnemy();
                    lastSpawnTime = Time.time;
                    nextSpawnDelay = Random.Range(0f, 5f);
                }
                
            }

            if (activeEnemies.Count <= 0)
            {
                EndDay();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartDay();
            }
        }

        
        
    }
    
    void SpawnEnemy()
    {
        lastSpawnTime = Time.time;
        float angleInDegrees = Random.Range(0f, 360f);
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

        float radius = Random.Range(40, 100);
        float x = Mathf.Cos(angleInRadians) * radius;
        float z = Mathf.Sin(angleInRadians) * radius;

        Vector3 spawnPosition = new Vector3(x, 0.1f, z);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();
        enemyComponent.gm = this.GetComponent<GameManager>();
        enemyComponent.target = enemyTarget;

        activeEnemies.Add(enemyComponent);
        spawnCount++;
    }
    void PrepareDay()
    {
         if(day==1)
         {
            Debug.Log(" gün 1 lo");
            StartCoroutine(CounterCoroutine4());
        }
        else
        {
            dayText.text = "Gün " + day.ToString();
            StartCoroutine(CounterCoroutine3());
        }

        
        
    }
    private IEnumerator CounterCoroutine3()
    {
        float duration = 3f;
        yield return new WaitForSeconds(2.0f);
        dayPanel.SetActive(true);
        Color color = DayImage.color;
        color.a = 0f;

        Color targetColor = DayImage.color;
        targetColor.a = 1f;
        float elapsedTime = 0;

     
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            DayImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        
        DayImage.color = targetColor;

    }

    private IEnumerator CounterCoroutine4()
    {
        float duration = 3f;
        dayPanel.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        
        Color color = DayImage.color;
        color.a = 1f;

        Color targetColor = DayImage.color;
        targetColor.a = 0f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            DayImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        DayImage.color = targetColor;

    }

    void StartDay()
    {
        GameObject[] rovers = GameObject.FindGameObjectsWithTag("Rover");

        foreach (GameObject rover in rovers)
        {
            rover.transform.position = Vector3.zero;
            rover.GetComponent<Rover>().enabled = true;
            
        }
        dayPanel.SetActive(false);

        isPlaying = true;
        spawnCount = 0;
        

    }

    void EndDay()
    {
        isPlaying = false;
        day++;
        PrepareDay();
        GameObject[] rovers = GameObject.FindGameObjectsWithTag("Rover");

        foreach (GameObject rover in rovers)
        {
            rover.GetComponent<Rover>().enabled = false;
            rover.transform.position = Vector3.zero;
        }
    }
    public void StartButton()
    {
        start = true;
        MainMenu.SetActive(false);
        Story1.SetActive(true);
        StartCoroutine(CounterCoroutine());
        autoFlip.Basla = true;
        
        
    }
    public void Start2()
    {
        Story3.SetActive(true);
        StartCoroutine(CounterCoroutine2());
    }
    private IEnumerator CounterCoroutine()
    {
        float counter = 0f;
        while (counter < 10f)
        {
            counter += Time.deltaTime;
            
            yield return null;
        }

        Story1.SetActive(false);
        Story2.SetActive(true);
        
       
    }
    private IEnumerator CounterCoroutine2()
    {
        float counter = 0f;
        while (counter < 10f)
        {
            counter += Time.deltaTime;
          
            yield return null;
        }

        Story1.SetActive(false);
        
        Story2.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void PauseButton()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MarketBackButton()
    {
        MarketMenu.SetActive(false);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Çýktý");
    }

    public void SettingsButton()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        GameOverMenu.SetActive(true);
        Debug.Log("Game Over");
    }



    public void AttackBase()
    {
        childCount = healthBar.transform.childCount;

        
        if (childCount > 0)
        {
           
            GameObject lastChild = healthBar.GetChild(childCount - 1).gameObject;
            
           
            Destroy(lastChild);

             
            
        }
        else if(childCount ==0)
        {
            GameOver();
        }
        

    }


}
