using Sirenix.OdinInspector;
using UnityEngine;
using DnVCorp.Systems;

namespace DnVCorp
{
    namespace Debugs
    {
        public class FPSDisplay : MonoBehaviour
        {
            [Space(10)]
            [Title("Options", null, TitleAlignments.Centered)]
            [EnumToggleButtons]
            [HideLabel]
            public OptionColor option;

            [Space(10)]
            [Title("Performance Colors", null, TitleAlignments.Centered)]
            [ShowIf("option", OptionColor.PerformanceView)]
            public Color GoodPerformance = Color.green;
            [ShowIf("option", OptionColor.PerformanceView)]
            public Color AvgPerformance = Color.yellow;
            [ShowIf("option", OptionColor.PerformanceView)]
            public Color BadPerformance = Color.red;

            [Space(10)]
            [Title("Color", null, TitleAlignments.Centered)]
            [ShowIf("option", OptionColor.Color)]
            public Color _color = Color.white;

            [Space(10)]
            [Title("Properties", null, TitleAlignments.Centered)]
            [Range(0.1f, 5f)]
            public float sizeScale = 1f;

            // Tmp Variables
            float deltaTime = 0.0f;

            #region Unity Behavior
            private void Update()
            {
                deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            }
            private void OnGUI()
            {
                int w = Screen.width, h = Screen.height;
                GUIStyle style = new GUIStyle();
                Rect rect = new Rect(0, 0, w, h * 3 / 100);
                style.alignment = TextAnchor.UpperLeft;
                style.fontSize = (int)(h * 3 * sizeScale / 100);
                float msec = deltaTime * 1000.0f;
                float fps = 1.0f / deltaTime;
                if (option == OptionColor.PerformanceView) style.normal.textColor = fps > 30f ? (fps > 45 ? GoodPerformance : AvgPerformance) : BadPerformance;
                else if (option == OptionColor.Color) style.normal.textColor = _color;
                string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                GUI.Label(rect, text, style);
            }
            #endregion
        }
    }
}
