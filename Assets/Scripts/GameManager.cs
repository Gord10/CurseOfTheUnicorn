using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentExp = 0;
    public int expRequirementForNextLevel = 10;
    
    public int[] levelExpRequirements; //Level 1's exp requirement is at 0th index
   
    private int playerLevel = 1;
    private bool isGameOver = false;
    private float timeWhenGameWasOver = 0;

    private void Awake()
    {
        instance = this;
        expRequirementForNextLevel = levelExpRequirements[0];
        Time.timeScale = 1f;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void ReportEnemyDeath()
    {
        currentExp++;
        
        if(currentExp >= expRequirementForNextLevel)
        {
            IncreaseLevel();
        }

        GameUi.instance.FillExperienceBar((float)currentExp / (float)expRequirementForNextLevel);
    }

    private void IncreaseLevel()
    {
        playerLevel++;
        BulletManager.instance.IncreaseFireSpeed();
        currentExp = 0;

        if(playerLevel < levelExpRequirements.Length)
        {
            expRequirementForNextLevel = levelExpRequirements[playerLevel - 1];
        }
        
        GameUi.instance.UpdateLevelText(playerLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            if(Input.anyKeyDown && Time.realtimeSinceStartup - timeWhenGameWasOver > 1f)
            {
                RestartScene();
            }
        }

#if UNITY_STANDALONE
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }

    public static void RestartScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void ReportGameOver()
    {
        isGameOver = true;
        GameUi.instance.ShowGameOverScreen();
        Time.timeScale = 0f;
        timeWhenGameWasOver = Time.realtimeSinceStartup;
    }
}
