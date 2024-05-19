using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AutoFlip autoFlip;
    public GameObject MainMenu;
    public GameObject Story1;
    public GameObject Story2;
    public GameObject Story3;
    public ResourceController resources;
    public float counter = 0;
   
  
    public bool start=false;

    public int day;
    public GameObject PauseMenu;
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
        
        dayText.text = "Gün " + day.ToString();

        dayPanel.SetActive(true);
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
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        
        //autoFlip.Basla = true;
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
        Debug.Log("Game Over");
    }



    public void AttackBase()
    {
        int childCount = healthBar.transform.childCount;

        
        if (childCount > 0)
        {
           
            GameObject lastChild = healthBar.GetChild(childCount - 1).gameObject;

           
            Destroy(lastChild);
        }
        else
        {
            GameOver();
        }

    }


}
