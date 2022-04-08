using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Aslan
{
    public class A_FpsCounter : MonoBehaviour
    {
        Text text;

        private float timeLeft = 1.0f; // Left time for current interval
        int lastFrameCount;
        static bool isShown = false;

        void Start()
        {
            text = GetComponent<Text>();
            //timeleft = updateInterval;
            text.text = " ";
            text.enabled = isShown;
            lastFrameCount = Time.frameCount;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F2))
            {
                isShown = !isShown;
                text.enabled = isShown;
            }

            timeLeft -= Time.unscaledDeltaTime;
            if(timeLeft <= 0.0f)
            {
                text.text = (Time.frameCount - lastFrameCount -1).ToString();
                lastFrameCount = Time.frameCount;
                timeLeft = 1.0f;
            }
        }
    }
}