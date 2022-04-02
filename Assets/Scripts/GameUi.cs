using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    public static GameUi instance;

    public Image healthBar, experienceBar;
    public Text timeText;
    public Text vibrationText;
    public Text levelText;
    public Text levelIncreaseText;
    public GameObject gameOverScreen;
    public Text survivedForSecondsText;

    private void Awake()
    {
        instance = this;
        FillHealthBar(1);
        FillExperienceBar(0);
        vibrationText.enabled = false;
        levelIncreaseText.enabled = false;
        gameOverScreen.SetActive(false);
    }

    public void UpdateLevelText(int newLevel)
    {
        levelText.text = "Level: " + newLevel.ToString();
        levelIncreaseText.enabled = true;
        levelIncreaseText.color = Color.white;
        StartCoroutine(FadeOutLevelIncreaseText());
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        survivedForSecondsText.text = survivedForSecondsText.text.Replace("#", TimeToText()); //Show how long the player survived
    }

    private IEnumerator FadeOutLevelIncreaseText()
    {
        float t = 3f;
        while(t > 0)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime);
            Color color = levelIncreaseText.color;
            color.a = t;
            levelIncreaseText.color = color;
            yield return new WaitForEndOfFrame();
        }

        levelIncreaseText.enabled = false;
    }

    public void FillHealthBar(float ratio)
    {
        healthBar.fillAmount = ratio;
    }

    public void FillExperienceBar(float ratio)
    {
        experienceBar.fillAmount = ratio;
    }

    private void FixedUpdate()
    {
        timeText.text = TimeToText();
    }

    private string TimeToText()
    {
        int minutes = (int)(Time.timeSinceLevelLoad / 60f);
        int seconds = (int)Time.timeSinceLevelLoad % 60;
        return minutes + ":" + seconds.ToString("00");
    }

    public void ShowVibrationInfo(bool value)
    {
        CancelInvoke("DisableVibrationText");
        vibrationText.text = "Vibration " + ((value) ? "ON" : "Off");
        vibrationText.enabled = true;
        Invoke("DisableVibrationText", 1);
    }

    private void DisableVibrationText()
    {
        vibrationText.enabled = false;
    }
}
