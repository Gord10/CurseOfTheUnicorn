using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : MonoBehaviour
{
    public static bool isVibrationEnabled = true;
    public static float cooldown = 0.5f;
    private static float lastTimeVibrated = 0; //Will use Time.time to store the last time we vibrated the gamepad
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

    public static void Vibrate()
    {
        if(Time.time - lastTimeVibrated > cooldown)
        {
            if (Gamepad.current != null && isVibrationEnabled)
            {
                float lowFrequency = 0.03f;
                float hiFrequency = 0.03f;
                Gamepad.current.SetMotorSpeeds(lowFrequency, hiFrequency);
                lastTimeVibrated = Time.time;
            }
        }
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
