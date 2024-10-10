using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DnVCorp
{
    namespace Tools
    {
        namespace Sound
        {
            [RequireComponent(typeof(UnityEngine.UI.Button))]
            public class Toggle : MonoBehaviour
            {
                bool isOn;

                #region Behaviors
                public void Setup(bool _isOn)
                {
                    isOn = _isOn;
                    Setting();
                }
                public void ToggleHandle()
                {
                    isOn = !isOn;
                    Setting();
                }
                private void Setting()
                {
                    if (isOn) SettingOn();
                    else SettingOff();
                }

                private void SettingOn()
                {

                }

                private void SettingOff()
                {

                }
                #endregion
            }
        }
    }
}
