using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTimeLimit = 600; //Spawn Death after this amount of seconds
    public int currentExp = 0;
    public int[] levelExpRequirements; //Level 1's exp requirement is at 0th index

    private enum State
    {
        IN_GAME,
        PAUSED,
        UPGRADE,
        GAME_OVER
    }

    private State state = State.IN_GAME;

    private int expRequirementForNextLevel = 10; //Player will reach the next level when they gain this much experience. We will read the necessary value from levelExpRequirements array 
    private int playerLevel = 1;
    //private bool isGameOver = false;
    private float timeWhenGameWasOver = 0; //Time.realtimeSinceStartup will be assigned to this value when game is over

    private void Awake()
    {
        instance = this;
        expRequirementForNextLevel = levelExpRequirements[0];
        Time.timeScale = 1f;

    }

    public bool IsGameOver()
    {
        return state == State.GAME_OVER;
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
        state = State.UPGRADE;
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
        if(state == State.GAME_OVER)
        {
            float threshold = 1f; //We do this check in order to avoid player accidentally skipping the game over screen by pressing a key
            if(Input.anyKeyDown && Time.realtimeSinceStartup - timeWhenGameWasOver > threshold)
            {
                RestartScene();
            }
        }

        bool isGamePadStartButtonPressed = false;

        if(Gamepad.current != null)
        {
            if(Gamepad.current.startButton.isPressed)
            {
                isGamePadStartButtonPressed = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) || isGamePadStartButtonPressed)
        {
            if(state == State.IN_GAME) //Pause the game
            {
                PauseScreenUi.instance.Open();
                state = State.PAUSED;
                Time.timeScale = 0f;
            }
        }
    }

    public static void RestartScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void ReportGameOver()
    {
        state = State.GAME_OVER;
        SfxManager.instance.PlayLoseSound();
        //isGameOver = true;
        GameUi.instance.ShowGameOverScreen();
        Time.timeScale = 0f;
        timeWhenGameWasOver = Time.realtimeSinceStartup;
    }

    public void ReportTouchOfDeath()
    {
        VibrationManager.StopVibration();
        SceneManager.LoadScene("Story4");
    }

    public void ResumeGame()
    {
        state = State.IN_GAME;
        Time.timeScale = 1f;
    }

}
