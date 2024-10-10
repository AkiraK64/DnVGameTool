using DnVCorp.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnVCorp
{
    namespace Tools
    {
        namespace BtnInteract
        {
            public class InteractableHandle : MonoBehaviour
            {
                protected bool preInteractable = true;

                public virtual void Handle(bool interactable)
                {

                }
            }
        }
    }
}
