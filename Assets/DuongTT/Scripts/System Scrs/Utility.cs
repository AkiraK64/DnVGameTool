using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnVCorp
{
    namespace Systems
    {
        public static class Utility
        {
            private static Vector2 SwitchTargetToAnchoredOfRoot(RectTransform target, RectTransform root)
            {
                Vector2 localPoint;
                Vector2 fromPivotDerivedOffset = new Vector2(target.rect.width * target.pivot.x + target.rect.xMin, target.rect.height * target.pivot.y + target.rect.yMin);
                Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, target.position);
                screenP += fromPivotDerivedOffset;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(root, screenP, null, out localPoint);
                Vector2 pivotDerivedOffset = new Vector2(root.rect.width * root.pivot.x + root.rect.xMin, root.rect.height * root.pivot.y + root.rect.yMin);
                return root.anchoredPosition + localPoint - pivotDerivedOffset;
            }
        }
    }
}
