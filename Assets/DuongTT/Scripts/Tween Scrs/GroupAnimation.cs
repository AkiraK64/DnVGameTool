using DnVCorp.Systems;
using DnVCorp.Tweening.LoopEffect;
using PrimeTween;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DnVCorp
{
    namespace Tweening
    {
        public class GroupAnimation : MonoBehaviour
        {
            [Space(100)]
            [Title("Configs")]
            [SerializeField] bool UseUnscaleTime;
            [SerializeField] bool UseFixedUpdate;
            [OnValueChanged(nameof(OnLoopsChanged))]
            [SerializeField] int Loops = 1;
            [ShowIf("$LoopModeShowIf")]
            [OnValueChanged(nameof(OnLoopModeChanged))]
            [InfoBox("Cannot use Incremental Mode", InfoMessageType.Warning)]
            [SerializeField] CycleMode LoopMode = CycleMode.Restart;
            [Space(20)]
            [Title("Effects")]
            [TypeFilter("GetFilteredTypeList")]
            [SerializeReference] public List<RootThread> GroupEffects = new List<RootThread>();

            Sequence sequence;

            public IEnumerable<Type> GetFilteredTypeList()
            {
                var q = typeof(RootThread).Assembly.GetTypes()
                              .Where(x => x != typeof(RootThread))
                              .Where(x => x != typeof(BaseThread))
                              .Where(x => typeof(RootThread).IsAssignableFrom(x));

                return q;
            }

            void OnLoopsChanged()
            {
                if (Loops == 0) Loops = 1;
                else if (Loops < 0) Loops = -1;
                if (Loops == 1) LoopMode = CycleMode.Restart;
            }
            bool LoopModeShowIf()
            {
                return Loops != 0 && Loops != 1;
            }
            void OnLoopModeChanged()
            {
                if (LoopMode == CycleMode.Incremental) LoopMode = CycleMode.Restart;
            }

            private void OnEnable()
            {
                if (sequence.isAlive) sequence.Stop();
                sequence = Sequence.Create(Loops, LoopMode, Ease.Default, UseUnscaleTime, UseFixedUpdate);
                foreach (var effect in GroupEffects)
                {
                    if (effect == null || effect.TargetIsNull()) continue;
                    effect.OnStart();
                    sequence = sequence.Group(effect.NewTween());
                }
            }

            private void OnDisable()
            {
                if (sequence.isAlive) sequence.Stop();
            }

            private void OnDestroy()
            {
                if (sequence.isAlive) sequence.Stop();
            }

            #region Editor
#if UNITY_EDITOR
            [OnInspectorGUI]
            private void OnInspectorGUI()
            {
                GUI.DrawTexture(new Rect(10, 30, 80, 80), (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Yellow.png", typeof(Texture2D)));
                GUI.DrawTexture(new Rect(100, 30, 80, 80), (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Blue.png", typeof(Texture2D)));
            }
#endif
            #endregion
        }
    }
}

namespace DnVCorp
{
    namespace Tweening
    {
        namespace LoopEffect
        {
            [Serializable]
            [LabelText("@$value.GetName()"), GUIColor("@$value.GetColor()")]
            public class RootThread
            {
                public virtual Tween NewTween()
                {
                    return new Tween();
                }
                public virtual bool TargetIsNull()
                {
                    return false;
                }
                public virtual void OnStart()
                {

                }
                public virtual string GetName()
                {
                    return "Base";
                }
                public virtual UnityEngine.Color GetColor()
                {
                    return UnityEngine.Color.cyan;
                }
            }
            public class Delay : RootThread
            {
                [Title("Configs", null, TitleAlignments.Centered)]
                [SerializeField] protected float Duration = 1;

                public override Tween NewTween()
                {
                    return Tween.Delay(Duration);
                }
                public override string GetName()
                {
                    return "Delay";
                }
            }
            public class BaseThread : RootThread
            {
                [Title("Configs", null, TitleAlignments.Centered)]
                [SerializeField] protected float Duration = 1;
                [SerializeField] protected Ease Ease = Ease.InOutSine;
                [ShowIf("$EaseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurve = AnimationCurve.Linear(0, 0, 1, 1);

                protected Tween thisTween;

                protected virtual bool EaseShowIf()
                {
                    return Ease == Ease.Custom;
                }
            }
            public class Move : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.position = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.Position(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "Null";
                    return target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }
            public class LocalMove : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.localPosition = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.LocalPosition(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "Null";
                    return target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }
            public class Rotate : BaseThread
            {
                [SerializeField] private RotateMode RotateMode;
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.eulerAngles = StartValue;
                }
                public override Tween NewTween()
                {
                    if (RotateMode == RotateMode.FastWay) return Tween.Rotation(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                    else if (RotateMode == RotateMode.RightWay) return Tween.EulerAngles(target, target.eulerAngles, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                    else return Tween.EulerAngles(target, target.eulerAngles, target.eulerAngles + EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "Null";
                    return target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }
            public class LocalRotate : BaseThread
            {
                [SerializeField] private RotateMode RotateMode;
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.localEulerAngles = StartValue;
                }
                public override Tween NewTween()
                {
                    if (RotateMode == RotateMode.FastWay) return Tween.LocalRotation(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                    else if (RotateMode == RotateMode.RightWay) return Tween.LocalEulerAngles(target, target.localEulerAngles, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                    else return Tween.LocalEulerAngles(target, target.localEulerAngles, target.localEulerAngles + EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "Null";
                    return target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }
            public class Scale : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private float StartValue;
                [SerializeField] private float EndValue;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.localScale = StartValue * Vector3.one;
                }
                public override Tween NewTween()
                {
                    return Tween.Scale(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "Null";
                    return target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }
            public class Anchor2D : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private Vector2 StartValue;
                [SerializeField] private Vector2 EndValue;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                public override void OnStart()
                {
                    target.anchoredPosition = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.UIAnchoredPosition(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "Null";
                    return target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }
            public class Anchor3D : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                public override void OnStart()
                {
                    target.anchoredPosition3D = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.UIAnchoredPosition3D(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "Null";
                    return target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }
            public class Color : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private UnityEngine.Color StartValue = UnityEngine.Color.white;
                [SerializeField] private UnityEngine.Color EndValue = UnityEngine.Color.white;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] UnityEngine.UI.Graphic target;

                public override void OnStart()
                {
                    target.color = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.Color(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "(UI) Null";
                    return "(UI) " + target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }            
            public class Fade : BaseThread
            {
                [PropertyOrder(1)]
                [Range(0f, 1f)]
                [SerializeField] private float StartValue = 1f;
                [Range(0f, 1f)]
                [SerializeField] private float EndValue = 1f;

                [Space(10)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] UnityEngine.UI.Graphic target;

                public override void OnStart()
                {
                    target.color = new UnityEngine.Color(target.color.r, target.color.g, target.color.b, StartValue);
                }
                public override Tween NewTween()
                {
                    return Tween.Alpha(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override string GetName()
                {
                    if (target == null) return "(UI) Null";
                    return "(UI) " + target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
            }           
        }
    }
}