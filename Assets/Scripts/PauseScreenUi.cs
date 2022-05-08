using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PauseScreenUi : MonoBehaviour
{
    public static PauseScreenUi instance;
    public GameObject quitButton, vibrationButton;
    public TMP_Text vibrationText;

    public void Init()
    {
        instance = this;
        Close();

#if UNITY_WEBGL
        //We don't need these buttons for WebGL
        quitButton.SetActive(false); 
        vibrationButton.SetActive(false);
#endif

    }

    public void Open()
    {
        gameObject.SetActive(true);
        SetVibrationText();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void SetVibrationText()
    {
        vibrationText.text = "Vibration: " + (VibrationManager.isVibrationEnabled ? "ON" : "OFF");
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();
        Close();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ResumeGame();
            Close();
        }
    }

    public void SwitchVibration()
    {
        VibrationManager.instance.SwitchVibration();
        SetVibrationText();
    }
}
