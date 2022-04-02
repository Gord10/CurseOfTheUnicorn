using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    public static GameUi instance;

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
}
