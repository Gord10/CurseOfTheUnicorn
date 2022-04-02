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
        if(Input.GetKeyDown(KeyCode.V))
        {
            isVibrationEnabled = !isVibrationEnabled;
            GameUi.instance.ShowVibrationInfo(isVibrationEnabled);
        }
    }

    public static void Vibrate()
    {
        if (Gamepad.current != null && isVibrationEnabled)
        {
            Gamepad.current.SetMotorSpeeds(0.1f, 0.1f);
        }
    }
}
