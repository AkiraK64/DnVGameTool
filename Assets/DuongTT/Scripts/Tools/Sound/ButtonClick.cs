using DnVCorp.Manager;
using UnityEngine;

namespace DnVCorp
{
    namespace Tools
    {
        namespace Sound
        {
            public class ButtonClick : MonoBehaviour
            {
                public void OnClick()
                {
                    SoundAndVibrateManager.Instance.PlaySFX("Button");
                    SoundAndVibrateManager.Instance.Vibrate();
                }
            }
        }
    }
}
