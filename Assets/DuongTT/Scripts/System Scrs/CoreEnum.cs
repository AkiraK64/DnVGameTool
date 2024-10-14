
using UnityEngine;

namespace DnVCorp
{
    namespace Systems
    {
        public enum OptionColor
        {
            Color,
            PerformanceView
        }

        [System.Flags]
        public enum OptionSetingSound
        {
            ToggleMusic = 1 << 0,
            ToggleSFX = 1 << 1,
            SliderMusic = 1 << 2,
            SliderSFX = 1 << 3,
            SliderMaster = 1 << 4,
            ToggleVibrate = 1 << 5
        }

        public enum RotateMode
        {
            FastWay,
            RightWay,
            AddAxis
        }

        public enum TweenAnimationEnable
        {
            None,
            Play,
            Restart
        }

        public enum TweenAnimationDisable
        {
            None,
            Pause,
            Rewind,
            Kill,
            Complete,
            DestroyGameObject
        }

        public enum TweenAnimationEffect
        {
            Transform,
            Rect,
            Image,
            Sprite,
            Material,
            Text,
            Light,
            Audio,
            Progress,
            Punch,
            Shake,
            Camera,
            Delay
        }

        [System.Flags]
        public enum TweenAnimationEvent
        {
            OnStart = 1 << 0,
            OnPause = 1 << 1,
            OnResume = 1 << 2,
            OnComplete = 1 << 3
        }

        public enum ScreenSpace
        {
            Overlay,
            Camera
        }
    }
}
