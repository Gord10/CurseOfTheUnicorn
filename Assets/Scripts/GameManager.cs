using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTimeLimit = 600; //Spawn Death after this amount of seconds
    public int currentExp = 0;
    public int[] levelExpRequirements; //Level 1's exp requirement is at 0th index

    private int expRequirementForNextLevel = 10; //Player will reach the next level when they gain this much experience. We will read the necessary value from levelExpRequirements array 
    private int playerLevel = 1;
    private bool isGameOver = false;
    private float timeWhenGameWasOver = 0; //Time.realtimeSinceStartup will be assigned to this value when game is over

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

    //Player has killed an enemy 
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
        SfxManager.instance.PlayLevelUpSound();
        Time.timeScale = 0;
        playerLevel++;
        currentExp = 0;

        if(playerLevel < levelExpRequirements.Length)
        {
            expRequirementForNextLevel = levelExpRequirements[playerLevel - 1];
        }
        
        GameUi.instance.UpdateLevelText(playerLevel);
        GameUi.instance.OpenUpgradeScreen();
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.instance.Invoke("SpawnDeath", gameTimeLimit);
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            float threshold = 1f; //We do this check in order to avoid player accidentally skipping the game over screen by pressing a key
            if(Input.anyKeyDown && Time.realtimeSinceStartup - timeWhenGameWasOver > threshold)
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
        SfxManager.instance.PlayLoseSound();
        isGameOver = true;
        GameUi.instance.ShowGameOverScreen();
        Time.timeScale = 0f;
        timeWhenGameWasOver = Time.realtimeSinceStartup;
    }

    public void ReportTouchOfDeath()
    {
        VibrationManager.StopVibration();
        SceneManager.LoadScene("Story4");
    }
}
