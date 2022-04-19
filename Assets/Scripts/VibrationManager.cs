using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;

    public static bool isVibrationEnabled = true;
    public static float cooldown = 0.5f;
    private static float lastTimeVibrated = 0; //Will use Time.time to store the last time we vibrated the gamepad

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) //Switch the vibration
        {
            isVibrationEnabled = !isVibrationEnabled;
            GameUi.instance.ShowVibrationInfo(isVibrationEnabled);
        }
    }

    public void SwitchVibration()
    {
        isVibrationEnabled = !isVibrationEnabled;

        if(isVibrationEnabled)
        {
            Vibrate();
            float waitTime = 0.1f;
            StartCoroutine(StopVibration(waitTime));
        }
    }

    public static void Vibrate()
    {
        if(Time.realtimeSinceStartup - lastTimeVibrated > cooldown)
        {
            if (Gamepad.current != null && isVibrationEnabled)
            {
                float lowFrequency = 0.03f;
                float hiFrequency = 0.03f;
                Gamepad.current.SetMotorSpeeds(lowFrequency, hiFrequency);
                lastTimeVibrated = Time.realtimeSinceStartup;
            }
        }
    }

    //Stops the vibration after waiting for a while
    private IEnumerator StopVibration(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        StopVibration();
    }

    public static void StopVibration()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    private void OnApplicationQuit()
    {
        StopVibration();
    }
}
