using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DnVCorp
{
    namespace Tools
    {
        namespace BtnInteract
        {
            public class InteractColorHandle : InteractableHandle
            {
                [Title("Color Handle")]
                [SerializeField] Image[] images = new Image[0];

                public override void Handle(bool interactable)
                {
                    base.Handle(interactable);
                    if (interactable == preInteractable) return;
                    preInteractable = interactable;
                    foreach(var image in images)
                    {
                        if(interactable) image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                        else image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
                    }
                }
            }
        }
    }
}
