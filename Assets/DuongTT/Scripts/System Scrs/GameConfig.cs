using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnVCorp
{
    namespace Systems
    {
        public class GameConfig : MonoBehaviour
        {
            private void Awake()
            {
                Application.targetFrameRate = 60;
                QualitySettings.vSyncCount = 0;
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
        }
    }
}
