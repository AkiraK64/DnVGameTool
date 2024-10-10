using UnityEngine;
using DnVCorp.Systems;
using Sirenix.OdinInspector;
using DnVCorp.Tweening;

namespace DnVCorp
{
    namespace Screens
    {
        public abstract class BaseScreen : MonoBehaviour
        {
            [Space(100)]
            [Title("Base Properties")]
            [Required]
            [SerializeField] Canvas canvas;
            [SerializeField] ScreenSpace renderMode;
            [ShowIf("@this.renderMode == ScreenSpace.Overlay")]
            [SerializeField] int sortOrder;
            [ShowIf("@this.renderMode == ScreenSpace.Camera")]
            [SerializeField] float planeDistance;
            [ShowIf("@this.renderMode == ScreenSpace.Camera")]
            [SerializeField] int orderInLayer;

            [Title("Advanced Properties")]
            [SerializeField] bool hasAnimation;
            [ShowIf("hasAnimation")]
            [Required]
            [SerializeField] OnceAnimation UIAnimation;

            #region Phase Handle
            private void Awake()
            {
                OnAwake();
            }

            protected virtual void OnAwake()
            {
                SetupCanvas();
            }

            protected void SetupCanvas()
            {
                switch (renderMode)
                {
                    case ScreenSpace.Overlay:
                        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.sortingOrder = sortOrder;
                        break;
                    case ScreenSpace.Camera:
                        canvas.renderMode = RenderMode.ScreenSpaceCamera;
                        canvas.worldCamera = Camera.main;
                        canvas.planeDistance = planeDistance;
                        canvas.sortingOrder = orderInLayer;
                        break;
                }
            }

            private void Start()
            {
                OnStart();
            }

            protected virtual void OnStart()
            {

            }

            private void OnEnable()
            {
                OnOpen();
            }

            protected virtual void OnOpen()
            {

            }

            private void OnDisable()
            {
                OnClose();
            }

            protected virtual void OnClose()
            {

            }
            #endregion

            #region Behavior
            public void Close()
            {
                ThePool.Despawn(gameObject);
            }

            public float CloseWithAnimation()
            {
                if (hasAnimation && UIAnimation != null) return UIAnimation.Close();
                Close();
                return 0f;
            }

            public void ChangeRenderCamera(Camera newCamera)
            {
                if(renderMode == ScreenSpace.Camera)
                {
                    canvas.worldCamera = newCamera;
                    canvas.planeDistance = planeDistance;
                    canvas.sortingOrder = orderInLayer;
                }
            }

            public void ChangeSortingOrder(int newSortOrder)
            {
                canvas.sortingOrder = newSortOrder;
            }
            #endregion

            #region Editor
#if UNITY_EDITOR
            // Editor
            [OnInspectorGUI]
            private void OnInspectorGUI()
            {
                GUI.DrawTexture(new Rect(10, 30, 80, 80), (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Yellow.png", typeof(Texture2D)));
                GUI.DrawTexture(new Rect(100, 30, 80, 80), (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Blue.png", typeof(Texture2D)));
            }
#endif
            #endregion
        }

        public class BaseScreen<T> : BaseScreen where T : BaseScreen
        {
            public static T Create()
            {
                return Create($"Screens/{typeof(T).Name}");
            }

            public static T Create(string path)
            {
                var prefab = Resources.Load<T>(path);
                var instance = ThePool.Spawn(prefab);
                instance.gameObject.SetActive(true);
                return instance;
            }
        }
    }
}
