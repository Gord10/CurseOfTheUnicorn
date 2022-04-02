using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    public static GameUi instance;
    public Text timeText;

    private void Awake()
    {
        instance = this;
        FillHealthBar(1);
    }

    public Image healthBar;

    public void FillHealthBar(float ratio)
    {
        healthBar.fillAmount = ratio;
    }

    private void FixedUpdate()
    {
        int minutes = (int) (Time.timeSinceLevelLoad / 60f);
        int seconds = (int) Time.timeSinceLevelLoad % 60;
        timeText.text = minutes + ":" + seconds.ToString("00");
    }
}
