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
            public class IconButton : BaseButton
            {
                [Space(10)]
                [Title("Icon Button")]
                public bool useTitle;
                [ShowIf("useTitle")]
                [Required]
                public TMP_Text title;

                public void SetText(string _title)
                {
                    if (useTitle && title != null) title.SetText(_title);
                }

                protected override void InteractController()
                {
                    base.InteractController();
                    if (useInteractHandle && interactHandle != null) interactHandle.Handle(interactable);
                }
            }
        }
    }
}
