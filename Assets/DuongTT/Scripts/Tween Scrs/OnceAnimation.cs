using DnVCorp.Tweening.OnceEffect;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using PrimeTween;
using DnVCorp.Systems;
using UnityEditor;

namespace DnVCorp
{
    namespace Tweening
    {
        public class OnceAnimation : MonoBehaviour
        {
            [Space(100)]
            [Title("Properties")]
            [SerializeField] bool JustOpen;
            [SerializeField] bool JustClose;
            [SerializeField] bool IgnoreTimescale;

            [Space(20)]
            [Title("Effects")]            
            [PropertySpace(SpaceBefore = 10, SpaceAfter = 20)]
            [HideIf("@this.JustClose && this.JustOpen")]
            [TypeFilter("GetFilteredTypeList")]
            [SerializeReference] public List<RootThread> GroupEffects = new List<RootThread>();

            [HorizontalGroup("row1", Title = "On Open Events")]
            [LabelText("Start")]
            [SerializeField] UnityEvent onStart = new UnityEvent();
            [HorizontalGroup("row1")]
            [LabelText("Complete")]
            [SerializeField] UnityEvent onComplete = new UnityEvent();

            [HorizontalGroup("row2", Title = "On Close Events")]
            [LabelText("Start")]
            [SerializeField] UnityEvent onReverseStart = new UnityEvent();
            [HorizontalGroup("row2")]
            [LabelText("Complete")]
            [SerializeField] UnityEvent onReverseComplete = new UnityEvent();

            Sequence sequence;

            public IEnumerable<Type> GetFilteredTypeList()
            {
                var q = typeof(RootThread).Assembly.GetTypes()
                              .Where(x => x != typeof(RootThread))
                              .Where(x => x != typeof(BaseThread))
                              .Where(x => typeof(RootThread).IsAssignableFrom(x));

                return q;
            }         

            private void OnEnable()
            {
                onStart?.Invoke();
                if (JustOpen)
                {
                    onComplete?.Invoke();
                    return;
                }
                if (sequence.isAlive) sequence.Stop();
                sequence = Sequence.Create(1, CycleMode.Restart, Ease.Default, IgnoreTimescale);
                foreach (var effect in GroupEffects)
                {
                    if (effect == null || effect.TargetIsNull()) continue;
                    effect.OnStart();
                    if(!effect.CanRightWay()) continue;
                    sequence = sequence.Group(effect.NewTween());
                }
                sequence = sequence.OnComplete(onComplete.Invoke);
            }

            private void OnDestroy()
            {
                if (sequence.isAlive) sequence.Stop();
            }

            public float Close()
            {
                onReverseStart?.Invoke();
                if (JustClose)
                {
                    onReverseComplete?.Invoke();
                    return 0f;
                }
                if (sequence.isAlive) sequence.Stop();
                sequence = Sequence.Create(1, CycleMode.Restart, Ease.Default, IgnoreTimescale);
                foreach (var effect in GroupEffects)
                {
                    if (effect == null || effect.TargetIsNull() || !effect.CanReverse()) continue;
                    sequence = sequence.Group(effect.ReverseTween());
                }
                sequence = sequence.OnComplete(onReverseComplete.Invoke);
                return sequence.durationTotal;
            }

            #region Editor
            [OnInspectorGUI]
            private void OnInspectorGUI()
            {
#if UNITY_EDITOR
                GUI.DrawTexture(new Rect(30, 30, 80, 80), (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Yellow.png", typeof(Texture2D)));
                GUI.DrawTexture(new Rect(120, 30, 80, 80), (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Blue.png", typeof(Texture2D)));
#endif
            }
            #endregion
        }
    }
}

namespace DnVCorp
{
    namespace Tweening
    {
        namespace OnceEffect
        {
            [Serializable]
            [LabelText("@$value.GetName()"), GUIColor("@$value.GetColor()")]
            public class RootThread
            {            
                public virtual Tween NewTween()
                {
                    return new Tween();
                }
                public virtual Tween ReverseTween()
                {
                    return new Tween();
                }                
                public virtual bool TargetIsNull()
                {
                    return false;
                }
                public virtual bool CanReverse()
                {
                    return false;
                }
                public virtual bool CanRightWay()
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
                [Title("On Open Configs", null, TitleAlignments.Centered)]
                [SerializeField] protected bool UseOpenEffect = false;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] protected float Duration = 1;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;

                public override Tween NewTween()
                {
                    return Tween.Delay(Duration);
                }
                public override Tween ReverseTween()
                {
                    return Tween.Delay(DurationReverse);
                }
                public override bool CanRightWay()
                {
                    return UseOpenEffect;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
                }
                public override string GetName()
                {
                    return "Delay";
                }
            }
            public class BaseThread : RootThread
            {
                [Title("On Open Configs", null, TitleAlignments.Centered)]
                [SerializeField] protected bool UseOpenEffect = false;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] protected float Duration = 1;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] protected float Delay = 0;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] protected Ease Ease = Ease.OutBack;
                [ShowIf("$EaseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurve = AnimationCurve.Linear(0, 0, 1, 1);

                protected virtual bool EaseShowIf()
                {
                    return Ease == Ease.Custom;
                }
                public override bool CanRightWay()
                {
                    return UseOpenEffect;
                }
            }
            public class Move : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [SerializeField] private Vector3 EndValueReverse;

                [PropertySpace(10,30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.position = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.Position(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    return Tween.Position(target, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
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
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }
            public class LocalMove : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [SerializeField] private Vector3 EndValueReverse;

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.localPosition = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.LocalPosition(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    return Tween.LocalPosition(target, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
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
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }
            public class Rotate : BaseThread
            {
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private RotateMode RotateMode;
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [SerializeField] private Vector3 EndValueReverse;

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.eulerAngles = StartValue;
                }
                public override Tween NewTween()
                {
                    if (RotateMode == RotateMode.FastWay) return Tween.Rotation(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                    else if (RotateMode == RotateMode.RightWay) return Tween.EulerAngles(target, target.eulerAngles, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                    else return Tween.EulerAngles(target, target.eulerAngles, target.eulerAngles + EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    if (RotateMode == RotateMode.FastWay) return Tween.Rotation(target, EndValueReverse, Duration, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                    else if (RotateMode == RotateMode.RightWay) return Tween.EulerAngles(target, target.eulerAngles, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                    else return Tween.EulerAngles(target, target.eulerAngles, target.eulerAngles + EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
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
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }
            public class LocalRotate : BaseThread
            {
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private RotateMode RotateMode;
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [SerializeField] private Vector3 EndValueReverse;

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.localEulerAngles = StartValue;
                }
                public override Tween NewTween()
                {
                    if (RotateMode == RotateMode.FastWay) return Tween.LocalRotation(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                    else if (RotateMode == RotateMode.RightWay) return Tween.LocalEulerAngles(target, target.localEulerAngles, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                    else return Tween.LocalEulerAngles(target, target.localEulerAngles, target.localEulerAngles + EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    if (RotateMode == RotateMode.FastWay) return Tween.LocalRotation(target, EndValueReverse, Duration, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                    else if (RotateMode == RotateMode.RightWay) return Tween.LocalEulerAngles(target, target.localEulerAngles, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                    else return Tween.LocalEulerAngles(target, target.localEulerAngles, target.localEulerAngles + EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
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
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }
            public class Scale : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private float StartValue;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private float EndValue;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [SerializeField] private float EndValueReverse;

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                public override void OnStart()
                {
                    target.localScale = StartValue * Vector3.one;
                }
                public override Tween NewTween()
                {
                    return Tween.Scale(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    return Tween.Scale(target, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
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
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }
            public class Anchor2D : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private Vector2 StartValue;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private Vector2 EndValue;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [SerializeField] private Vector2 EndValueReverse;

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                public override void OnStart()
                {
                    target.anchoredPosition = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.UIAnchoredPosition(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    return Tween.UIAnchoredPosition(target, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
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
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }
            public class Anchor3D : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private Vector3 StartValue;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private Vector3 EndValue;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [SerializeField] private Vector3 EndValueReverse;

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                public override void OnStart()
                {
                    target.anchoredPosition3D = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.UIAnchoredPosition3D(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    return Tween.UIAnchoredPosition3D(target, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
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
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }                     
            public class Color : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private UnityEngine.Color StartValue = UnityEngine.Color.white;
                [ShowIf("UseOpenEffect", true)]
                [SerializeField] private UnityEngine.Color EndValue = UnityEngine.Color.white;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [SerializeField] private UnityEngine.Color EndValueReverse = UnityEngine.Color.white;

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] UnityEngine.UI.Graphic target;

                public override void OnStart()
                {
                    target.color = StartValue;
                }
                public override Tween NewTween()
                {
                    return Tween.Color(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    return Tween.Color(target, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
                }
                public override string GetName()
                {
                    if (target == null) return "(UI) Null";
                    return "(UI)" + target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }
            public class Fade : BaseThread
            {
                [PropertyOrder(1)]
                [Range(0f, 1f)]
                [SerializeField] private float StartValue = 1f;
                [ShowIf("UseOpenEffect", true)]
                [Range(0f, 1f)]
                [SerializeField] private float EndValue = 1f;

                [Space(10)]
                [Title("On Close Configs", null, TitleAlignments.Centered)]
                [SerializeField] private bool UseCloseEffect;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Duration")]
                [SerializeField] private float DurationReverse = 1f;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Delay")]
                [SerializeField] private float DelayReverse;
                [ShowIf("UseCloseEffect", true)]
                [LabelText("Ease")]
                [SerializeField] private Ease EaseReverse = Ease.InBack;
                [LabelText("Custom Curve")]
                [ShowIf("$EaseReverseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurveReverse = AnimationCurve.Linear(0, 0, 1, 1);
                [ShowIf("UseCloseEffect", true)]
                [LabelText("EndValue")]
                [Range(0f, 1f)]
                [SerializeField] private float EndValueReverse = 1f;

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] UnityEngine.UI.Graphic target;

                public override void OnStart()
                {
                    target.color = new UnityEngine.Color(target.color.r, target.color.g, target.color.b, StartValue);
                }
                public override Tween NewTween()
                {                  
                    return Tween.Alpha(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    return Tween.Alpha(target, EndValueReverse, DurationReverse, (EaseReverse == Ease.Custom) ? CustomCurveReverse : EaseReverse, 1, CycleMode.Restart, DelayReverse);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return UseCloseEffect;
                }
                public override string GetName()
                {
                    if (target == null) return "(UI) Null";
                    return "(UI)" + target.name;
                }
                public override UnityEngine.Color GetColor()
                {
                    if (target == null) return UnityEngine.Color.yellow;
                    return base.GetColor();
                }
                bool EaseReverseShowIf()
                {
                    return EaseReverse == Ease.Custom;
                }
            }
            public class Writing : BaseThread
            {
                [PropertyOrder(1)]
                [SerializeField] private string StartValue = "Hello";

                [PropertySpace(10, 30)]
                [Title("References", null, TitleAlignments.Centered)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] TMPro.TMP_Text target;

                public override void OnStart()
                {
                    target.SetText(StartValue);
                }
                public override Tween NewTween()
                {
                    return Tween.TextMaxVisibleCharacters(target, 0, target.textInfo.characterCount, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, 1, CycleMode.Restart, Delay);
                }
                public override Tween ReverseTween()
                {
                    return Tween.Delay(Duration);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                public override bool CanReverse()
                {
                    return false;
                }
                public override string GetName()
                {
                    if (target == null) return "(UI) Null";
                    return "(UI)" + target.name;
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