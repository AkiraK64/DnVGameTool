using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

namespace DnVCorp
{
    namespace Tools
    {
        namespace Btn
        {
            public class SimpleButton : BaseButton
            {
                [Space(10)]
                [Title("Simple Button")]
                [Required]
                public TMP_Text title;

                public void SetText(string _title)
                {
                    if(title != null)  title.SetText(_title);
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
