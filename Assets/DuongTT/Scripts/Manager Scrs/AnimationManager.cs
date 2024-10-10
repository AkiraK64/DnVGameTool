
using System.Collections.Generic;
using UnityEngine;

namespace DnVCorp
{
    namespace Manager
    {
        public static class AnimationManager
        {
            public static Dictionary<string, int> hashTable = new Dictionary<string, int>();

            #region Behavior
            public static void Play(Animator animator, string stateName, int layer = 0, float normalizedTime = 0.0f)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    animator.Play(hashTable[stateName], layer, normalizedTime);
                }
            }
            public static float GetFloat(Animator animator, string stateName)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    return animator.GetFloat(hashTable[stateName]);
                }
                return 0.0f;
            }
            public static void SetFloat(Animator animator, string stateName, float value)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    animator.SetFloat(hashTable[stateName], value);
                }
            }
            public static bool GetBool(Animator animator, string stateName)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    return animator.GetBool(hashTable[stateName]);
                }
                return false;
            }
            public static void SetBool(Animator animator, string stateName, bool value)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    animator.SetBool(hashTable[stateName], value);
                }
            }
            public static int GetInteger(Animator animator, string stateName)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    return animator.GetInteger(hashTable[stateName]);
                }
                return 0;
            }
            public static void SetInteger(Animator animator, string stateName, int value)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    animator.SetInteger(hashTable[stateName], value);
                }
            }
            public static void ResetTrigger(Animator animator, string stateName)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    animator.ResetTrigger(hashTable[stateName]);
                }
            }
            public static void ResetAllTrigger(Animator animator)
            {
                if (animator != null)
                {
                    foreach (var param in animator.parameters)
                    {
                        if (param.type == AnimatorControllerParameterType.Trigger)
                        {
                            if (!hashTable.ContainsKey(param.name)) hashTable.Add(param.name, Animator.StringToHash(param.name));
                            animator.ResetTrigger(hashTable[param.name]);
                        }
                    }
                }
            }
            public static void SetTrigger(Animator animator, string stateName)
            {
                if (animator != null && stateName != null)
                {
                    if (!hashTable.ContainsKey(stateName)) hashTable.Add(stateName, Animator.StringToHash(stateName));
                    animator.SetTrigger(hashTable[stateName]);
                }
            }
            #endregion
        }
    }
}
