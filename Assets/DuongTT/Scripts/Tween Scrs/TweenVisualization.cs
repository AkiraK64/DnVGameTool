using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;
using System.Collections.Generic;
using DnVCorp.Systems;
using UnityEngine.Events;
using DnVCorp.Tweening.TweenEffect;
using TMPro;
using PrimeTween;
using UnityEditor;

namespace DnVCorp
{
    namespace Tweening
    {
        public class TweenVisualization : MonoBehaviour
        {
            [Space(100)]
            [Title("Phase", null, TitleAlignments.Centered)]
            [LabelText("On Enable")]
            [OnValueChanged(nameof(OnEnablePhaseChanged))]
            [SerializeField] private TweenAnimationEnable PhaseEnable = TweenAnimationEnable.None;
            [LabelText("On Disable")]
            [SerializeField] private TweenAnimationDisable PhaseDisable = TweenAnimationDisable.None;

            [Space(10)]
            [Title("Effect", null, TitleAlignments.Centered)]
            [OnValueChanged(nameof(OnFilterChanged))]
            [SerializeField] private TweenAnimationEffect filter = TweenAnimationEffect.Transform;
            [Space(5)]
            [TypeFilter("GetFilteredTypeList")]
            [SerializeReference] public RootClass effectClass;

            private string effectName = "Transform";

            public IEnumerable<Type> GetFilteredTypeList()
            {
                var q = typeof(RootClass).Assembly.GetTypes()     
                              .Where(x => x != typeof(BaseClass))
                              .Where(x => typeof(RootClass).IsAssignableFrom(x))                    
                              .Where(x => x.Name.Contains(effectName));

                return q;
            }

            [Space(20)]
            [Title("Events", null, TitleAlignments.Centered)]
            [EnumToggleButtons, HideLabel]
            [SerializeField] private TweenAnimationEvent eventOption;

            [Space(10)]
            [ShowIf("@eventOption.HasFlag(TweenAnimationEvent.OnStart)")]
            [SerializeField] private UnityEvent event_OnStart = new UnityEvent();

            [Space(20)]
            [ShowIf("@eventOption.HasFlag(TweenAnimationEvent.OnPause)")]
            [SerializeField] private UnityEvent event_OnPause = new UnityEvent();

            [Space(10)]
            [ShowIf("@eventOption.HasFlag(TweenAnimationEvent.OnResume)")]
            [SerializeField] private UnityEvent event_OnResume = new UnityEvent();

            [Space(10)]
            [ShowIf("@eventOption.HasFlag(TweenAnimationEvent.OnComplete)")]
            [SerializeField] private UnityEvent event_OnComplete = new UnityEvent();

            private void OnFilterChanged()
            {
                effectName = filter.ToString() + "_";
            }

            private void OnEnable()
            {
                if (effectClass == null) return;

                if (effectClass.TargetIsNull()) return;

                if(PhaseEnable == TweenAnimationEnable.Play)
                {
                    if(eventOption.HasFlag(TweenAnimationEvent.OnResume)) event_OnResume?.Invoke();
                    effectClass.Play();
                }
                else if (PhaseEnable == TweenAnimationEnable.Restart)
                {
                    effectClass.SetupProperties();

                    if (effectClass.GetEventOption() != eventOption)
                    {
                        effectClass.SetEventOption(eventOption);
                        effectClass.SetAction(event_OnStart.Invoke, event_OnComplete.Invoke);
                    }

                    effectClass.StopTween();
                    effectClass.PlayTween();
                }
            }

            private void OnDisable()
            {
                if (effectClass == null) return;

                if (effectClass.TargetIsNull()) return;

                if (PhaseDisable == TweenAnimationDisable.Pause)
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnPause)) event_OnPause?.Invoke();
                    effectClass.Pause();
                }
                else if (PhaseDisable == TweenAnimationDisable.Rewind)
                {
                    effectClass.StopTween();
                    effectClass.RewindProperties();
                }
                else if (PhaseDisable == TweenAnimationDisable.Kill)
                {
                    effectClass.StopTween();
                }
                else if (PhaseDisable == TweenAnimationDisable.Complete)
                {
                    effectClass.CompleteTween();
                }
                else if (PhaseDisable == TweenAnimationDisable.DestroyGameObject)
                {
                    effectClass.StopTween();
                    Destroy(gameObject);
                }
            }

            private void OnDestroy()
            {
                if (effectClass == null) return;

                if (effectClass.TargetIsNull()) return;

                effectClass.StopTween();
            }

            private void OnEnablePhaseChanged()
            {
                if (PhaseEnable == TweenAnimationEnable.None) PhaseDisable = TweenAnimationDisable.None;
                if (PhaseEnable == TweenAnimationEnable.Play) PhaseDisable = TweenAnimationDisable.Pause;
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
        namespace TweenEffect
        {
            [Serializable]
            public class RootClass
            {
                public virtual void PlayTween()
                {

                }

                public virtual void SetupProperties()
                {

                }

                public virtual void RewindProperties()
                {

                }

                public virtual bool TargetIsNull()
                {
                    return false;
                }

                //public virtual string DurationText()
                //{
                //    return "Duration";
                //}

                public virtual void Play()
                {

                }

                public virtual void Pause()
                {

                }

                public virtual void StopTween()
                {

                }

                public virtual void CompleteTween()
                {

                }

                public virtual TweenAnimationEvent GetEventOption()
                {
                    return TweenAnimationEvent.OnStart;
                }

                public virtual void SetEventOption(TweenAnimationEvent option)
                {

                }

                public virtual void SetAction(Action eventStart, Action eventComplete)
                {
                    
                }
            }
            public class Delay_Time : RootClass
            {                
                [SerializeField] protected float Duration = 1;
                [SerializeField] protected bool IgnoreTimescale = false;

                protected TweenAnimationEvent eventOption;
                protected Tween thisTween;

                protected Action event_OnStart;
                protected Action event_OnComplete;

                public override void SetAction(Action eventStart, Action eventComplete)
                {
                    event_OnStart = eventStart;
                    event_OnComplete = eventComplete;
                }
                public override TweenAnimationEvent GetEventOption()
                {
                    return eventOption;
                }
                public override void SetEventOption(TweenAnimationEvent option)
                {
                    eventOption = option;
                }
                public override void Play()
                {
                    if (thisTween.isAlive && thisTween.isPaused) thisTween.isPaused = false;
                    else PlayTween();
                }
                public override void Pause()
                {
                    if (thisTween.isAlive) thisTween.isPaused = true;
                }
                public override void StopTween()
                {
                    thisTween.Stop();
                }
                public override void CompleteTween()
                {
                    thisTween.Complete();
                }
                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Delay(Duration, eventOption.HasFlag(TweenAnimationEvent.OnComplete) ? event_OnComplete : null, IgnoreTimescale);
                }
            }
            public class BaseClass : RootClass
            {
                [SerializeField] protected float Duration = 1;
                [SerializeField] protected float Delay = 0;
                [SerializeField] protected bool IgnoreTimescale = false;
                [SerializeField] protected Ease Ease = Ease.OutBack;
                [ShowIf("$EaseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurve = AnimationCurve.Linear(0,0,1,1);
                [OnValueChanged(nameof(OnLoopsChanged))]
                [SerializeField] protected int Loops = 1;
                [ShowIf("$LoopModeShowIf")]
                [SerializeField] protected CycleMode LoopMode = CycleMode.Restart;

                protected TweenAnimationEvent eventOption;
                protected Tween thisTween;

                protected Action event_OnStart;
                protected Action event_OnComplete;

                public override void SetAction(Action eventStart, Action eventComplete)
                {
                    event_OnStart = eventStart;
                    event_OnComplete = eventComplete;
                }
                public override TweenAnimationEvent GetEventOption()
                {
                    return eventOption;
                }
                public override void SetEventOption(TweenAnimationEvent option)
                {
                    eventOption = option;
                }
                private void OnLoopsChanged()
                {
                    if (Loops == 0) Loops = 1;
                    else if (Loops < 0) Loops = -1;
                    if (Loops == 1) LoopMode = CycleMode.Restart;
                }
                protected virtual bool LoopModeShowIf()
                {
                    return Loops != 0 && Loops != 1;
                }
                protected virtual bool EaseShowIf()
                {
                    return Ease == Ease.Custom;
                }
                public override void Play()
                {
                    if (thisTween.isAlive && thisTween.isPaused) thisTween.isPaused = false;
                    else PlayTween();
                }
                public override void Pause()
                {
                    if (thisTween.isAlive) thisTween.isPaused = true;
                }
                public override void StopTween()
                {
                    thisTween.Stop();
                }
                public override void CompleteTween()
                {
                    thisTween.Complete();
                }
            }
            public class Transform_Move : BaseClass
            {
                [SerializeField] private Vector3 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 startPosition;                

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Position(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.position = startPosition;
                }
                public override void SetupProperties()
                {
                    startPosition = target.position;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }             
            }
            public class Transform_LocalMove : BaseClass
            {
                [SerializeField] private Vector3 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 startLocalPosition;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.LocalPosition(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.position = startLocalPosition;
                }
                public override void SetupProperties()
                {
                    startLocalPosition = target.position;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }               
            }
            public class Transform_Rotate : BaseClass
            {
                [SerializeField] private RotateMode RotateMode;
                [SerializeField] private Vector3 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 startAngle;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    if(RotateMode == RotateMode.FastWay) thisTween = Tween.Rotation(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    else if(RotateMode == RotateMode.RightWay) thisTween = Tween.EulerAngles(target, target.eulerAngles, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    else if(RotateMode == RotateMode.AddAxis) thisTween = Tween.EulerAngles(target, target.eulerAngles, target.eulerAngles + EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.eulerAngles = startAngle;
                }
                public override void SetupProperties()
                {
                    startAngle = target.eulerAngles;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }               
            }
            public class Transform_LocalRotate : BaseClass
            {
                [SerializeField] private RotateMode RotateMode;
                [SerializeField] private Vector3 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 startLocalAngle;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    if (RotateMode == RotateMode.FastWay) thisTween = Tween.LocalRotation(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    else if (RotateMode == RotateMode.RightWay) thisTween = Tween.LocalEulerAngles(target, target.localEulerAngles, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    else if (RotateMode == RotateMode.AddAxis) thisTween = Tween.LocalEulerAngles(target, target.localEulerAngles, target.localEulerAngles + EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.localEulerAngles = startLocalAngle;
                }
                public override void SetupProperties()
                {
                    startLocalAngle = target.localEulerAngles;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }              
            }
            public class Transform_Scale : BaseClass
            {
                [SerializeField] private float EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 startScale;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Scale(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.localScale = startScale;
                }
                public override void SetupProperties()
                {
                    startScale = target.localScale;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }          
            public class Rect_Anchor2D : BaseClass
            {
                [SerializeField] private Vector2 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                Vector2 startAnchor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.UIAnchoredPosition(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.anchoredPosition = startAnchor;
                }
                public override void SetupProperties()
                {
                    startAnchor = target.anchoredPosition;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Rect_Anchor3D : BaseClass
            {
                [SerializeField] private Vector3 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                Vector3 startAnchor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.UIAnchoredPosition3D(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.anchoredPosition3D = startAnchor;
                }
                public override void SetupProperties()
                {
                    startAnchor = target.anchoredPosition3D;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Rect_AnchorMax : BaseClass
            {
                [SerializeField] private Vector2 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                Vector2 startAnchor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.UIAnchorMax(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.anchorMax = startAnchor;
                }
                public override void SetupProperties()
                {
                    startAnchor = target.anchorMax;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Rect_AnchorMin : BaseClass
            {
                [SerializeField] private Vector2 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                Vector2 startAnchor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.UIAnchorMin(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.anchorMin = startAnchor;
                }
                public override void SetupProperties()
                {
                    startAnchor = target.anchorMin;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Rect_OffsetMax : BaseClass
            {
                [SerializeField] private Vector2 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                Vector2 startAnchor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.UIOffsetMax(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.offsetMax = startAnchor;
                }
                public override void SetupProperties()
                {
                    startAnchor = target.offsetMax;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Rect_OffsetMin : BaseClass
            {
                [SerializeField] private Vector2 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                Vector2 startAnchor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.UIOffsetMin(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.offsetMin = startAnchor;
                }
                public override void SetupProperties()
                {
                    startAnchor = target.offsetMin;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Rect_Pivot : BaseClass
            {
                [SerializeField] private Vector2 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                Vector2 startAnchor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.UIPivot(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.pivot = startAnchor;
                }
                public override void SetupProperties()
                {
                    startAnchor = target.pivot;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Rect_SizeDelta : BaseClass
            {
                [SerializeField] private Vector2 EndValue;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] RectTransform target;

                Vector2 startAnchor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.UISizeDelta(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.sizeDelta = startAnchor;
                }
                public override void SetupProperties()
                {
                    startAnchor = target.sizeDelta;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Image_Color : BaseClass
            {              
                [SerializeField] private Color EndValue= Color.white;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] UnityEngine.UI.Image target;

                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Color(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.color = startColor;
                }
                public override void SetupProperties()
                {
                    startColor = target.color;
                }
                public override bool TargetIsNull()
                {                   
                    return target == null;
                }
            }
            public class Image_Fade : BaseClass
            {
                [Range(0f, 1f)]
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] UnityEngine.UI.Image target;

                float startFade;
                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Alpha(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    startColor.a = startFade;
                    target.color = startColor;                    
                }
                public override void SetupProperties()
                {
                    startColor = target.color;
                    startFade = startColor.a;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Sprite_Color : BaseClass
            {
                [SerializeField] private Color EndValue = Color.white;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] SpriteRenderer target;

                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Color(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.color = startColor;
                }
                public override void SetupProperties()
                {
                    startColor = target.color;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Sprite_Fade : BaseClass
            {
                [Range(0f, 1f)]
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] SpriteRenderer target;

                float startFade;
                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Alpha(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    startColor.a = startFade;
                    target.color = startColor;
                }
                public override void SetupProperties()
                {
                    startColor = target.color;
                    startFade = startColor.a;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Material_Color : BaseClass
            {
                [SerializeField] private Color EndValue = Color.white;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Renderer target;

                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.MaterialColor(target.material, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.material.color = startColor;
                }
                public override void SetupProperties()
                {
                    startColor = target.material.color;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Material_Fade : BaseClass
            {
                [Range(0f, 1f)]
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Renderer target;

                float startFade;
                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.MaterialAlpha(target.material, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    startColor.a = startFade;
                    target.material.color = startColor;
                }
                public override void SetupProperties()
                {
                    startColor = target.material.color;
                    startFade = startColor.a;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Material_MainTextureOffset : BaseClass
            {
                [Range(0f, 1f)]
                [SerializeField] private Vector2 EndValue = Vector2.one;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Renderer target;

                Vector2 startValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.MaterialMainTextureOffset(target.material, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.material.mainTextureOffset = startValue;
                }
                public override void SetupProperties()
                {
                    startValue = target.material.mainTextureOffset;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Material_MainTextureScale : BaseClass
            {
                [Range(0f, 1f)]
                [SerializeField] private Vector2 EndValue = Vector2.one;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Renderer target;

                Vector2 startValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.MaterialMainTextureScale(target.material, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.material.mainTextureScale = startValue;
                }
                public override void SetupProperties()
                {
                    startValue = target.material.mainTextureScale;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Material_Property : BaseClass
            {
                [Range(0f, 1f)]
                [SerializeField] private int PropertyId = 1;
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Renderer target;

                float startValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.MaterialProperty(target.material, PropertyId, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.material.SetFloat(PropertyId, startValue);
                }
                public override void SetupProperties()
                {
                    startValue = target.material.GetFloat(PropertyId);
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Text_Color : BaseClass
            {
                [SerializeField] private Color EndValue = Color.white;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] TMP_Text target;

                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Color(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.color = startColor;
                }
                public override void SetupProperties()
                {
                    startColor = target.color;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Text_Fade : BaseClass
            {
                [Range(0f, 1f)]
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] TMP_Text target;

                float startFade;
                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.Alpha(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    startColor.a = startFade;
                    target.color = startColor;
                }
                public override void SetupProperties()
                {
                    startColor = target.color;
                    startFade = startColor.a;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Text_Writing : RootClass
            {
                [SerializeField] protected float CharsPerSecond = 1;
                [SerializeField] protected float Delay = 0;
                [SerializeField] protected bool IgnoreTimescale = false;
                [SerializeField] protected Ease Ease = Ease.OutBack;
                [ShowIf("$EaseShowIf")]
                [SerializeField] protected AnimationCurve CustomCurve = AnimationCurve.Linear(0, 0, 1, 1);
                [OnValueChanged(nameof(OnLoopsChanged))]
                [SerializeField] protected int Loops = 1;
                [ShowIf("$LoopModeShowIf")]
                [SerializeField] protected CycleMode LoopMode = CycleMode.Restart;

                protected TweenAnimationEvent eventOption;
                protected Tween thisTween;

                protected Action event_OnStart;
                protected Action event_OnComplete;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] TMP_Text target;

                string startInfo;

                public override void SetAction(Action eventStart, Action eventComplete)
                {
                    event_OnStart = eventStart;
                    event_OnComplete = eventComplete;
                }
                public override TweenAnimationEvent GetEventOption()
                {
                    return eventOption;
                }
                public override void SetEventOption(TweenAnimationEvent option)
                {
                    eventOption = option;
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
                bool EaseShowIf()
                {
                    return Ease == Ease.Custom;
                }

                public override void Play()
                {
                    if (thisTween.isAlive && thisTween.isPaused) thisTween.isPaused = false;
                    else PlayTween();
                }
                public override void Pause()
                {
                    if (thisTween.isAlive) thisTween.isPaused = true;
                }
                public override void StopTween()
                {
                    thisTween.Stop();
                }
                public override void CompleteTween()
                {
                    thisTween.Complete();
                }

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    int characterCount = target.textInfo.characterCount;
                    float duration = characterCount / CharsPerSecond;
                    thisTween = Tween.TextMaxVisibleCharacters(target, 0, characterCount, duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.SetText(startInfo);
                }
                public override void SetupProperties()
                {
                    startInfo = target.text;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Light_Color : BaseClass
            {
                [SerializeField] private Color EndValue = Color.white;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Light target;

                Color startColor;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.LightColor(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.color = startColor;
                }
                public override void SetupProperties()
                {
                    startColor = target.color;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Light_Intensity : BaseClass
            {
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Light target;

                float startValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.LightIntensity(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.intensity = startValue;
                }
                public override void SetupProperties()
                {
                    startValue = target.intensity;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Light_ShadowStrength : BaseClass
            {
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Light target;

                float startValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.LightShadowStrength(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.shadowStrength = startValue;
                }
                public override void SetupProperties()
                {
                    startValue = target.shadowStrength;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Audio_Volume : BaseClass
            {
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] AudioSource target;

                float startValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.AudioVolume(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.volume = startValue;
                }
                public override void SetupProperties()
                {
                    startValue = target.volume;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Audio_Pitch : BaseClass
            {
                [SerializeField] private float EndValue = 1f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] AudioSource target;

                float startValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.AudioPitch(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.pitch = startValue;
                }
                public override void SetupProperties()
                {
                    startValue = target.pitch;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }                                 
            public class Punch_Position : BaseClass
            {
                [SerializeField] private Vector3 EndValue = Vector3.one;
                [Range(1, 50)]
                [SerializeField] private int Vibrato = 10;
                [InfoBox("0 is no recoil, 1 is full recoil")]
                [Range(0f, 1f)]
                [SerializeField] private float Elasticity = 1f;
                [InfoBox("Can not using custom tween for fade out effect", InfoMessageType.Warning)]
                [SerializeField] private bool FadeOut = true;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.PunchLocalPosition(target, EndValue, Duration, Vibrato, FadeOut, (Ease == Ease.Custom) ? Ease.Linear : Ease, 1 - Elasticity, Loops, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.localPosition = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.localPosition;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                protected override bool EaseShowIf()
                {
                    return false;
                }
            }
            public class Punch_Rotation : BaseClass
            {
                [SerializeField] private Vector3 EndValue = Vector3.one;
                [Range(1, 50)]
                [SerializeField] private int Vibrato = 10;
                [InfoBox("0 is no recoil, 1 is full recoil")]
                [Range(0f, 1f)]
                [SerializeField] private float Elasticity = 1f;
                [InfoBox("Can not using custom tween for fade out effect", InfoMessageType.Warning)]
                [SerializeField] private bool FadeOut = true;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.PunchLocalRotation(target, EndValue, Duration, Vibrato, FadeOut, (Ease == Ease.Custom) ? Ease.Linear : Ease, 1 - Elasticity, Loops, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.localEulerAngles = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.localEulerAngles;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                protected override bool EaseShowIf()
                {
                    return false;
                }
            }
            public class Punch_Scale : BaseClass
            {
                [SerializeField] private Vector3 EndValue = Vector3.one;
                [Range(1, 50)]
                [SerializeField] private int Vibrato = 10;
                [InfoBox("0 is no recoil, 1 is full recoil")]
                [Range(0f, 1f)]
                [SerializeField] private float Elasticity = 1f;

                [InfoBox("Can not using custom tween for fade out effect", InfoMessageType.Warning)]
                [SerializeField] private bool FadeOut = true;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.PunchScale(target, EndValue, Duration, Vibrato, FadeOut, (Ease == Ease.Custom) ? Ease.Linear : Ease, 1 - Elasticity, Loops, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.localScale = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.localScale;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                protected override bool EaseShowIf()
                {
                    return false;
                }
            }
            public class Shake_Position : BaseClass
            {
                [SerializeField] private Vector3 EndValue = Vector3.one;
                [Range(1, 50)]
                [SerializeField] private int Vibrato = 10;
                [InfoBox("0 is no recoil, 1 is full recoil")]
                [Range(0f, 1f)]
                [SerializeField] private float Elasticity = 1f;
                [InfoBox("Can not using custom tween for fade out effect", InfoMessageType.Warning)]
                [SerializeField] private bool FadeOut = true;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.ShakeLocalPosition(target, EndValue, Duration, Vibrato, FadeOut, (Ease == Ease.Custom) ? Ease.Linear : Ease, 1 - Elasticity, Loops, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.localPosition = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.localPosition;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                protected override bool EaseShowIf()
                {
                    return false;
                }
            }
            public class Shake_Rotation : BaseClass
            {
                [SerializeField] private Vector3 EndValue = Vector3.one;
                [Range(1, 50)]
                [SerializeField] private int Vibrato = 10;
                [InfoBox("0 is no recoil, 1 is full recoil")]
                [Range(0f, 1f)]
                [SerializeField] private float Elasticity = 1f;
                [InfoBox("Can not using custom tween for fade out effect", InfoMessageType.Warning)]
                [SerializeField] private bool FadeOut = true;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.ShakeLocalRotation(target, EndValue, Duration, Vibrato, FadeOut, (Ease == Ease.Custom) ? Ease.Linear : Ease, 1 - Elasticity, Loops, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.localEulerAngles = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.localEulerAngles;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                protected override bool EaseShowIf()
                {
                    return false;
                }
            }
            public class Shake_Scale : BaseClass
            {
                [SerializeField] private Vector3 EndValue = Vector3.one;
                [Range(1, 50)]
                [SerializeField] private int Vibrato = 10;
                [InfoBox("0 is no recoil, 1 is full recoil")]
                [Range(0f, 1f)]
                [SerializeField] private float Elasticity = 1f;
                [InfoBox("Can not using custom tween for fade out effect", InfoMessageType.Warning)]
                [SerializeField] private bool FadeOut = true;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Transform target;

                Vector3 StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.ShakeScale(target, EndValue, Duration, Vibrato, FadeOut, (Ease == Ease.Custom) ? Ease.Linear : Ease, 1 - Elasticity, Loops, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.localScale = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.localScale;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
                protected override bool EaseShowIf()
                {
                    return false;
                }
            }
            public class Camera_Color : BaseClass
            {
                [SerializeField] private Color EndValue = Color.white;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Camera target;

                Color StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.CameraBackgroundColor(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.backgroundColor = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.backgroundColor;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Camera_Far : BaseClass
            {
                [SerializeField] private float EndValue = 100f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Camera target;

                float StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.CameraFarClipPlane(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.farClipPlane = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.farClipPlane;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Camera_Near : BaseClass
            {
                [SerializeField] private float EndValue = 0.5f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Camera target;

                float StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.CameraNearClipPlane(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.nearClipPlane = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.nearClipPlane;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Camera_FOV : BaseClass
            {
                [Range(1f, 179f)]
                [SerializeField] private float EndValue = 60f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Camera target;

                float StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.CameraFieldOfView(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.fieldOfView = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.fieldOfView;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Camera_OrthoSize : BaseClass
            {
                [SerializeField] private float EndValue = 5f;

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Camera target;

                float StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.CameraOrthographicSize(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.orthographicSize = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.orthographicSize;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Camera_PixelRect : BaseClass
            {
                [SerializeField] private Rect EndValue = new Rect(0,0,1,1);

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Camera target;

                Rect StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.CameraPixelRect(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }
                public override void RewindProperties()
                {
                    target.pixelRect = StartValue;
                }
                public override void SetupProperties()
                {
                    StartValue = target.pixelRect;
                }
                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }
            public class Camera_Rect : BaseClass
            {
                [SerializeField] private Rect EndValue = new Rect(0, 0, 1, 1);

                [PropertySpace(SpaceBefore = 20)]
                [Required("No valid Component was found for the selected animation")]
                [SerializeField] Camera target;

                Rect StartValue;

                public override void PlayTween()
                {
                    if (eventOption.HasFlag(TweenAnimationEvent.OnStart)) event_OnStart?.Invoke();
                    thisTween = Tween.CameraRect(target, EndValue, Duration, (Ease == Ease.Custom) ? CustomCurve : Ease, Loops, LoopMode, Delay, 0.0f, IgnoreTimescale);
                    if (eventOption.HasFlag(TweenAnimationEvent.OnComplete)) thisTween = thisTween.OnComplete(event_OnComplete);
                }

                public override void RewindProperties()
                {
                    target.rect = StartValue;
                }

                public override void SetupProperties()
                {
                    StartValue = target.rect;
                }

                public override bool TargetIsNull()
                {
                    return target == null;
                }
            }                                         
        }
    }
}

