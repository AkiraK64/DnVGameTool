using UnityEngine;

namespace DnVCorp
{
    namespace Systems
    {
        public static class NewPlayerPrefs
        {
            #region Base
            public static T Load<T>(string _name, T _defaultValue)
            {
                if(!PlayerPrefs.HasKey(_name))
                {
                    return _defaultValue;
                }
                var json = PlayerPrefs.GetString(_name);
                return JsonUtility.FromJson<T>(json);
            }
            public static void Save(string _name, object _data)
            {
                var json = JsonUtility.ToJson(_data);
                PlayerPrefs.SetString(_name, json);
            }
            #endregion

            #region Function
            public static int GetInt(string _name, int _defaultValue)
            {
                return PlayerPrefs.GetInt(_name, _defaultValue);
            }
            public static void SetInt(string _name, int _value)
            {
                PlayerPrefs.SetInt(_name, _value);
            }
            public static string GetString(string _name, string _defaultValue)
            {
                return PlayerPrefs.GetString(_name, _defaultValue);
            }
            public static void SetString(string _name, string _value)
            {
                PlayerPrefs.SetString(_name, _value);
            }
            public static float GetFloat(string _name, float _defaultValue)
            {
                return PlayerPrefs.GetFloat(_name, _defaultValue);
            }
            public static void SetFloat(string _name, float _value)
            {
                PlayerPrefs.SetFloat(_name, _value);
            }
            public static bool GetBool(string _name, bool _defaultValue)
            {
                return PlayerPrefs.GetInt(_name, _defaultValue ? 1 : 0) == 1 ? true : false;
            }
            public static void SetBool(string _name, bool _value)
            {
                PlayerPrefs.SetInt(_name, _value ? 1 : 0);
            }
            public static TimeStruct GetTime(string _name, TimeStruct _defaultValue)
            {
                return Load(_name, _defaultValue);
            }
            public static void SetTime(string _name, TimeStruct _value)
            {
                Save(_name, _value);
            }
            public static Vector3 GetVector3(string _name, Vector3 _defaultValue)
            {
                return Load(_name, _defaultValue);
            }
            public static void SetVector3(string _name, Vector3 _value)
            {
                Save(_name, _value);
            }
            public static Vector3 GetVector2(string _name, Vector2 _defaultValue)
            {
                return Load(_name, _defaultValue);
            }
            public static void SetVector2(string _name, Vector2 _value)
            {
                Save(_name, _value);
            }
            public static Vector3 GetVector4(string _name, Vector4 _defaultValue)
            {
                return Load(_name, _defaultValue);
            }
            public static void SetVector4(string _name, Vector4 _value)
            {
                Save(_name, _value);
            }
            public static Color GetColor(string _name, Color _defaultValue)
            {
                return Load(_name, _defaultValue);
            }
            public static void SetColor(string _name, Color _value)
            {
                Save(_name, _value);
            }
            #endregion
        }
    }
}