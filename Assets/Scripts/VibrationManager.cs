using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : MonoBehaviour
{
    public static bool isVibrationEnabled = true;

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
        if (Gamepad.current != null && isVibrationEnabled)
        {
            float lowFrequency = 0.03f;
            float hiFrequency = 0.03f;
            Gamepad.current.SetMotorSpeeds(lowFrequency, hiFrequency);
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
