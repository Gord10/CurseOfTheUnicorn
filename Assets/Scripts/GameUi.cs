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

    private void Awake()
    {
        instance = this;
        FillHealthBar(1);
        FillExperienceBar(0);
        vibrationText.enabled = false;
    }

    public void UpdateLevelText(int newLevel)
    {
        levelText.text = "Level: " + newLevel.ToString();
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
        int minutes = (int) (Time.timeSinceLevelLoad / 60f);
        int seconds = (int) Time.timeSinceLevelLoad % 60;
        timeText.text = minutes + ":" + seconds.ToString("00");
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
