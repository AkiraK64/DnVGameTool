using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PrimeTween;
using TMPro;
using UnityEditor;
using DnVCorp.Tools.BtnInteract;

namespace DnVCorp
{
    namespace Tools
    {
        namespace Btn
        {
            public class BaseButton : MonoBehaviour, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler
            {
                protected int pointID = -1;
                protected bool isInside = false;
                protected bool isPressed = false;
                protected bool interactable = true;

                [Space(100)]
                [Title("Properties")]
                [InfoBox("Content's scale always is 1", InfoMessageType.Warning)]
                [SerializeField] protected Transform _content;
                [SerializeField] protected Image targetGraphic;

                [Space(10)]
                [Title("Feature", "Interact Handle")]
                [SerializeField] protected bool useInteractHandle = false;
                [ShowIf("useInteractHandle")]
                [Required]
                [SerializeField] protected InteractableHandle interactHandle;

                [Space(10)]
                [Title("Feature", "Special Effect")]
                [SerializeField] protected bool useSpecialEffect = false;
                [ShowIf("useSpecialEffect")]
                [SerializeField] protected float factor = 1.1f;
                [ShowIf("useSpecialEffect")]
                [SerializeField] protected float duration = 0.5f;

                [Space(10)]
                [Title("Events")]
                public UnityEvent onClick = new UnityEvent();

                protected Tween effectTween;

                #region
                [OnInspectorGUI]
                private void OnInspectorGUI()
                {
#if UNITY_EDITOR
                    GUI.DrawTexture(new Rect(30, 30, 80, 80), (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Yellow.png", typeof(Texture2D)));
                    GUI.DrawTexture(new Rect(120, 30, 80, 80), (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Blue.png", typeof(Texture2D)));
#endif
                }
                #endregion

                // Effect Handle
                protected virtual void OnEnable()
                {
                    InteractController();
                }
                protected virtual void OnDisable()
                {
                    if (effectTween.isAlive) effectTween.Stop();
                    _content.localScale = Vector3.one;
                }
                public void OnPointerDown(PointerEventData eventData)
                {
                    if (!interactable) return;
                    if (!isPressed)
                    {
                        isPressed = true;
                        pointID = eventData.pointerId;
                        PressEffect();
                    }
                }
                private void PressEffect()
                {
                    if (effectTween.isAlive) effectTween.Stop();
                    effectTween = Tween.Scale(_content, 0.9f, 0.15f, Ease.OutBack, 1, CycleMode.Restart, 0f, 0f, true);
                }
                public void OnPointerUp(PointerEventData eventData)
                {
                    if (!interactable) return;
                    if (isPressed && pointID == eventData.pointerId)
                    {
                        isPressed = false;
                        if (isInside) onClick?.Invoke();
                        if (useSpecialEffect) BackToNormalAndRepeat();
                        else BackToNormal();
                    }
                }
                private void BackToNormal()
                {
                    if (effectTween.isAlive) effectTween.Stop();
                    effectTween = Tween.Scale(_content, 1f, 0.15f, Ease.OutBack, 1, CycleMode.Restart, 0f, 0f, true);
                }
                public void OnPointerEnter(PointerEventData eventData)
                {
                    isInside = true;
                }
                public void OnPointerExit(PointerEventData eventData)
                {
                    isInside = false;
                }
                public void PlaySpecialEffect()
                {
                    if (!useSpecialEffect) return;
                    if (effectTween.isAlive) effectTween.Stop();
                    effectTween = Tween.Scale(_content, 1f, factor, duration, Ease.InOutSine, -1, CycleMode.Yoyo, 0f, 0f, true);
                }
                private void BackToNormalAndRepeat()
                {
                    if (effectTween.isAlive) effectTween.Stop();
                    effectTween = Tween.Scale(_content, 1f, 0.15f, Ease.OutBack, 1, CycleMode.Restart, 0f, 0f, true)
                                       .OnComplete(this, target => target.PlaySpecialEffect());
                }

                [PropertySpace(20)]
                [Title("Action")]
                [Button(ButtonHeight = 30)]
                public void FixSize()
                {
                    var thisRect = gameObject.GetComponent<RectTransform>();
                    var graphicRect = targetGraphic.gameObject.GetComponent<RectTransform>();
                    thisRect.sizeDelta = graphicRect.localScale.x * _content.localScale.x * graphicRect.sizeDelta;
                }


                // Button Behavior
                public bool Interactable
                {
                    set
                    {
                        interactable = value;
                        InteractController();
                    }
                }

                protected virtual void InteractController()
                {
                    
                }
            }
        }
    }
}
