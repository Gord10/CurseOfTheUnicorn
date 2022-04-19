using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreenUi : MonoBehaviour
{
    public static PauseScreenUi instance;
    public GameObject quitButton;

    public void Init()
    {
        instance = this;
        Close();

#if UNITY_WEBGL
        quitButton.SetActive(false); //We don't need a quit button for WebGL
#endif

    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
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
}
