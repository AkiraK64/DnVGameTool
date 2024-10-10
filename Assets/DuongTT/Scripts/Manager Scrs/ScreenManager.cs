
using DnVCorp.Screens;
using System.Collections.Generic;
using UnityEngine;

namespace DnVCorp
{
    namespace Manager
    {
        public static class ScreenManager
        {
            static Dictionary<string, object> screenDict = new Dictionary<string, object>();

            public static T0 OpenScreen<T0>() where T0 : BaseScreen
            {               
                string key = typeof(T0).Name;

                if (screenDict.ContainsKey(key))
                {
                    if(screenDict[key] != null)
                    {
                        T0 screen = screenDict[key] as T0;
                        if(screen != null)
                        {
                            if (screen.gameObject != null && !screen.gameObject.activeSelf) screen.gameObject.SetActive(true);
#if UNITY_EDITOR
                            Debug.Log(string.Format("<color={0}>{1}</color>", "green", "POSTMAN MESSAGE: Successful! Open existed screen"));
#endif
                            return screen;
                        }
                        else
                        {
                            T0 newScreen3 = BaseScreen<T0>.Create();
                            screenDict[key] = newScreen3;
#if UNITY_EDITOR
                            Debug.Log(string.Format("<color={0}>{1}</color>", "cyan", "POSTMAN MESSAGE: Successful! Open new screen"));
#endif
                            return newScreen3;
                        }
                    }
                    else
                    {
                        T0 newScreen2 = BaseScreen<T0>.Create();
                        screenDict[key] = newScreen2;
#if UNITY_EDITOR
                        Debug.Log(string.Format("<color={0}>{1}</color>", "cyan", "POSTMAN MESSAGE: Successful! Open new screen"));
#endif
                        return newScreen2;
                    }
                }

                T0 newScreen = BaseScreen<T0>.Create();                
                screenDict.Add(key, newScreen);
#if UNITY_EDITOR
                Debug.Log(string.Format("<color={0}>{1}</color>", "cyan", "POSTMAN MESSAGE: Successful! Open new screen"));
#endif
                return newScreen;
            }

            public static T0 GetScreen<T0>() where T0 : BaseScreen
            {
                string key = typeof(T0).Name;

                if (screenDict.ContainsKey(key))
                {
                    if (screenDict[key] != null)
                    {
                        T0 screen = screenDict[key] as T0;
                        if (screen != null)
                        {
                            if (screen.gameObject != null && !screen.gameObject.activeSelf) screen.gameObject.SetActive(true);
#if UNITY_EDITOR
                            Debug.Log(string.Format("<color={0}>{1}</color>", "green", "POSTMAN MESSAGE: Successful! This screen existed"));
#endif
                            return screen;
                        }
                        else
                        {
#if UNITY_EDITOR
                            Debug.Log(string.Format("<color={0}>{1}</color>", "red", "POSTMAN MESSAGE: Failed! This screen does not exist"));
#endif
                        }
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.Log(string.Format("<color={0}>{1}</color>", "red", "POSTMAN MESSAGE: Failed! This screen does not exist"));
#endif
                    }
                }

#if UNITY_EDITOR
                Debug.Log(string.Format("<color={0}>{1}</color>", "red", "POSTMAN MESSAGE: Failed! This screen does not exist"));
#endif
                return null;
            }

            public static void CloseScreen<T0>() where T0 : BaseScreen
            {
                string key = typeof(T0).Name;

                if (screenDict.ContainsKey(key))
                {
                    if (screenDict[key] != null)
                    {
                        T0 screen = screenDict[key] as T0;
                        if (screen != null)
                        {
#if UNITY_EDITOR
                            Debug.Log(string.Format("<color={0}>{1}</color>", "green", "POSTMAN MESSAGE: Successful! Close done"));
#endif
                            screen.Close();
                        }
                        else
                        {
#if UNITY_EDITOR
                            Debug.Log(string.Format("<color={0}>{1}</color>", "red", "POSTMAN MESSAGE: Failed! This scene does not existed"));
#endif
                        }
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.Log(string.Format("<color={0}>{1}</color>", "red", "POSTMAN MESSAGE: Failed! This scene does not existed"));
#endif
                    }
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("<color={0}>{1}</color>", "red", "POSTMAN MESSAGE: Failed! This scene does not existed"));
#endif
                }
            }          
        }
    }
}
