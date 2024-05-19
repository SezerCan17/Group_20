using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int day;
    public GameObject PauseMenu;
    public GameObject MarketMenu;

    private void Awake()
    {
        PauseMenu.SetActive(false);
    }
    private void Update()
    {
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
        
    }
    public void StartButton()
    {
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
        day = 0;
    }

    public void NextWeek()
    {
        day++;
    }



}
