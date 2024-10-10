using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnVCorp
{
    namespace GlobalHandle
    {
        public static class ThePostman
        {
            public static Dictionary<string, object> messageBook = new Dictionary<string, object>();

            public static int Count
            {
                get
                {
                    return messageBook.Count;
                }
            }

            public static Dictionary<string, object>.KeyCollection Keys
            {
                get
                {
                    return messageBook.Keys;
                }
            }

            public static Dictionary<string, object>.ValueCollection Values
            {
                get
                {
                    return messageBook.Values;
                }
            }

            public static void Register<T0>(string id, Action<T0> action)
            {
                if (id == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "red", "POSTMAN MESSAGE: Failed! key is null"));
#endif
                    return;
                }
                if (messageBook.ContainsKey(id))
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "cyan", "POSTMAN MESSAGE: Key=" + id + " has new action"));
#endif

                    messageBook[id] = action;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "green", "POSTMAN MESSAGE: Created, key=" + id));
#endif
                    messageBook.Add(id, action);
                }
            }

            public static void Register(string id, Action action)
            {
                if (id == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "red", "POSTMAN MESSAGE: Failed! key is null"));
#endif
                    return;
                }
                if (messageBook.ContainsKey(id))
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "cyan", "POSTMAN MESSAGE: Key=" + id + " has new action"));
#endif
                    messageBook[id] = action;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "green", "POSTMAN MESSAGE: Created, key=" + id));
#endif
                    messageBook.Add(id, action);
                }
            }

            public static void UnRegister(string id)
            {
                if (messageBook.ContainsKey(id)) messageBook.Remove(id);
            }

            public static void Clear()
            {
                messageBook.Clear();
            }

            public static void Execute<T0>(string id, T0 param)
            {
                if (messageBook.ContainsKey(id)) (messageBook[id] as Action<T0>)?.Invoke(param);
            }

            public static void Execute(string id)
            {
                if (messageBook.ContainsKey(id)) (messageBook[id] as Action)?.Invoke();
            }

            public static bool HasKey(string id)
            {
                return messageBook.ContainsKey(id);
            }
        }
    }
}