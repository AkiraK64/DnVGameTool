using Sirenix.OdinInspector;
using UnityEngine;
using DnVCorp.Tools.BtnAnimation;
using TMPro;

namespace DnVCorp
{
    namespace Tools
    {
        namespace Btn
        {
            public class AnimationButton : BaseButton
            {
                [Space(10)]
                [Title("Animation")]
                [Required]
                [SerializeField] AnimationIcon iconAnimator;
                public bool useTitle;
                [ShowIf("useTitle")]
                [Required]
                public TMP_Text title;

                //Effect
                protected override void OnEnable()
                {
                    base.OnEnable();
                    if(interactable && iconAnimator != null) iconAnimator.PlayFromZero();
                }

                protected override void OnDisable()
                {
                    base.OnDisable();
                    if(iconAnimator != null) iconAnimator.StopAndReset();
                }

                public void SetText(string _title)
                {
                    if (useTitle && title != null) title.SetText(_title);
                }

                protected override void InteractController()
                {
                    base.InteractController();
                    if(iconAnimator != null) iconAnimator.StopImmediately();
                    if (useInteractHandle && interactHandle != null) interactHandle.Handle(interactable);                    
                }
            }
        }
    }
}
