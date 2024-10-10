using System;
using System.Collections.Generic;
using UnityEngine;

namespace DnVCorp
{
    namespace GlobalHandle
    {
        public static class MatchMaker<T0, T1>
        {
            public static Dictionary<T0, T1> matchBooks;

            public static bool DictionaryIsNull
            {
                get
                {
                    return matchBooks == null;
                }
            }
            public static int Count
            {
                get
                {
                    if (matchBooks == null) return 0;
                    return matchBooks.Count;
                }
            }
            public static Dictionary<T0, T1>.KeyCollection Keys
            {
                get
                {
                    if (matchBooks == null) return null;
                    return matchBooks.Keys;
                }
            }
            public static Dictionary<T0, T1>.ValueCollection Values
            {
                get
                {
                    if (matchBooks == null) return null;
                    return matchBooks.Values;
                }
            }
            public static void Add(T0 key, T1 value)
            {
                if (key == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "red", "MATCHING MESSAGE: Failed! key is null"));
#endif
                    return;
                }

                Assign();

                if (matchBooks.ContainsKey(key))
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "cyan", "MATCHING MESSAGE: Replace value"));
#endif
                    matchBooks[key] = value;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "green", "MATCHING MESSAGE: Created new key"));
#endif
                    matchBooks.Add(key, value);
                }
            }
            public static void Remove(T0 key)
            {
                if (matchBooks == null) return;
                if (matchBooks.ContainsKey(key)) matchBooks.Remove(key);
            }
            public static void Clear()
            {
                if(matchBooks == null) return;
                matchBooks.Clear();
            }
            public static T1 GetValue(T0 _key)
            {
                if (matchBooks !=  null && matchBooks.ContainsKey(_key)) return matchBooks[_key];
                return default(T1);
            }
            public static void Assign()
            {
                if (matchBooks == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "yellow", "MATCHING MESSAGE: Assign new dictionary"));
#endif
                    matchBooks = new Dictionary<T0, T1>();
                }
            }
            public static void Delete()
            {
                if (matchBooks != null) matchBooks.Clear();
                matchBooks = null;
            }
        }
    }
}
