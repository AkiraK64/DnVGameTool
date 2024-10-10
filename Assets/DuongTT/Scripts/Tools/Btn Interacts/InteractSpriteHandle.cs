using DnVCorp.Systems;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DnVCorp
{
    namespace Tools
    {
        namespace BtnInteract
        {
            public class InteractSpriteHandle : InteractableHandle
            {
                [Title("Sprite Handle")]
                [TableList]
                [SerializeField] InteractableSpriteStruct[] images = new InteractableSpriteStruct[0];

                public override void Handle(bool interactable)
                {
                    base.Handle(interactable);
                    if (interactable == preInteractable) return;
                    preInteractable = interactable;
                    foreach (var image in images)
                    {
                        image.sourceImage.sprite = interactable ? image.onSprite : image.offSprite;
                    }
                }
            }
        }
    }
}
