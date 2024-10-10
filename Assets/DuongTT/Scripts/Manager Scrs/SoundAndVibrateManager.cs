using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using DnVCorp.Datas;
using System.Collections.Generic;
using DnVCorp.Systems;

namespace DnVCorp
{
    namespace Manager
    {
        public class SoundAndVibrateManager : MonoBehaviour
        {
            public static SoundAndVibrateManager Instance;

            [Space(100)]
            [Title("Option", null, TitleAlignments.Centered)]
            [EnumToggleButtons]
            [HideLabel]
            [SerializeField] private OptionSetingSound option;

            [Space(10)]
            [Title("Music Source", null, TitleAlignments.Centered)]
            [Required]
            public AudioSource musicSource;

            [Space(10)]
            [Title("Sfx Source", null, TitleAlignments.Centered)]
            [Required]
            public AudioSource sfxSource;

            [Space(10)]
            [Title("Audio Assets", null, TitleAlignments.Centered)]
            [Required]
            [SerializeField] private AudioSO audioAsset;

            [Space(10)]
            [Title("Vibration", null, TitleAlignments.Centered)]
            [ReadOnly]
            public bool vibrate;
            public long miliseconds = 100;

            private Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();

            #region Editor
            // Editor
            [OnInspectorGUI]
            private void OnInspectorGUI()
            {
#if UNITY_EDITOR
                GUI.DrawTexture(new Rect(30, 30, 80, 80), (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Yellow.png", typeof(Texture2D)));
                GUI.DrawTexture(new Rect(120, 30, 80, 80), (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/DuongTT/Logo/Ixalag Blue.png", typeof(Texture2D)));
#endif
            }
            #endregion

            #region Unity Behavior
            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    clipDict = audioAsset.GenerateDict();
                    Init();
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            #endregion

            #region Behavior
            void Init()
            {
                if (option.HasFlag(OptionSetingSound.ToggleMusic))
                {
                    musicSource.mute = !DataManager.Get_MusicOn();
                }
                if (option.HasFlag(OptionSetingSound.ToggleSFX))
                {
                    sfxSource.mute = !DataManager.Get_SFXOn();
                }
                if (option.HasFlag(OptionSetingSound.ToggleVibrate))
                {
                    vibrate = DataManager.Get_VibrateOn();
                }
                if (option.HasFlag(OptionSetingSound.SliderMusic))
                {
                    musicSource.volume = DataManager.Get_MusicVolume() * DataManager.Get_MasterVolume();
                }
                if (option.HasFlag(OptionSetingSound.SliderSFX))
                {
                    sfxSource.volume = DataManager.Get_SFXVolume() * DataManager.Get_MasterVolume();
                }
                if (option.HasFlag(OptionSetingSound.SliderMaster))
                {
                    musicSource.volume = DataManager.Get_MusicVolume() * DataManager.Get_MasterVolume();
                    sfxSource.volume = DataManager.Get_SFXVolume() * DataManager.Get_MasterVolume();
                }
            }
            AudioClip GetClip(string name)
            {
                if (clipDict.ContainsKey(name)) return clipDict[name];
                return audioAsset.defaultClip;
            }
            public void PlaySFX(string nameOfSfx)
            {
                sfxSource.PlayOneShot(GetClip(nameOfSfx));
            }
            public void PlayMusic(string nameOfMusic)
            {
                musicSource.Stop();
                musicSource.clip = GetClip(nameOfMusic);
                musicSource.Play();
            }
            public void StopMusic()
            {
                musicSource.Stop();
            }
            public void Vibrate(long miliseconds)
            {
                if (vibrate) Vibrator.Vibrate(miliseconds);
            }
            public void Vibrate()
            {
                if (vibrate) Vibrator.Vibrate(miliseconds);
            }
            #endregion
        }
    }
}