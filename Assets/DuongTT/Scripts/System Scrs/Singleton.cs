using UnityEngine;

namespace DnVCorp
{
    namespace Systems
    {
        public class Singleton<T> : MonoBehaviour where T : Singleton<T>
        {
            private static object m_Lock = new object();
            private static T m_Instance;

            public static T Instance
            {
                get
                {
                    lock (m_Lock)
                    {
                        if (m_Instance == null)
                        {
                            // Search for existing instance.
                            m_Instance = (T)FindObjectOfType(typeof(T));

                            // Create new instance if one doesn't already exist.
                            if (m_Instance == null)
                            {
                                // Need to create a new GameObject to attach the singleton to.
                                var singletonObject = new GameObject();
                                m_Instance = singletonObject.AddComponent<T>();
                                singletonObject.name = typeof(T).Name + " (Singleton)";
                                // Make instance persistent.
                                DontDestroyOnLoad(singletonObject);
                            }
                        }

                        return m_Instance;
                    }
                }
            }

            protected virtual void Awake()
            {
                if (m_Instance == null)
                {
                    m_Instance = (T)this;
                    DontDestroyOnLoad(gameObject);
                    OnInit();
                }
                else if (m_Instance != this)
                {
                    Destroy(gameObject);
                }
            }

            protected virtual void OnInit()
            {

            }
        }
    }
}