using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DnVCorp;
using DnVCorp.Systems;
using DnVCorp.Manager;

namespace DnVCorp
{
    namespace Tools
    {
        namespace Sound
        {
            public class SoundAndVibrateSetting : MonoBehaviour
            {
                #region Base Properties;
                [PropertySpace(SpaceBefore = 10f, SpaceAfter = 10f)]
                [Title("Setting", null, TitleAlignments.Centered)]
                [EnumToggleButtons]
                [HideLabel]
                [SerializeField] private OptionSetingSound option;
                #endregion

                #region Toggle Music Properties;
                [Title("Toggle Music", null, TitleAlignments.Centered)]
                [ShowIf("@this.option.HasFlag(OptionSetingSound.ToggleMusic)")]
                [Required]
                [SerializeField] private Toggle toggleMusic;
                #endregion

                #region Toggle SFX Properties;
                [ShowIf("@this.option.HasFlag(OptionSetingSound.ToggleSFX)")]
                [Required]
                [SerializeField] private Toggle toggleSFX;
                #endregion

                #region Toggle Vibrate Properties;
                [ShowIf("@this.option.HasFlag(OptionSetingSound.ToggleVibrate)")]
                [Required]
                [SerializeField] private Toggle toggleVibrate;
                #endregion

                #region Slider Music Properties;
                [ShowIf("@this.option.HasFlag(OptionSetingSound.SliderMusic)")]
                [Required]
                [SerializeField] private Slider sliderMusic;
                #endregion

                #region Slider SFX Properties;
                [ShowIf("@this.option.HasFlag(OptionSetingSound.SliderSFX)")]
                [Required]
                [SerializeField] private Slider sliderSFX;
                #endregion

                #region Slider Master Properties;
                [ShowIf("@this.option.HasFlag(OptionSetingSound.SliderMaster)")]
                [Required]
                [SerializeField] private Slider sliderMaster;
                #endregion

                #region TMP Variables
                private bool musicOn = true;
                private bool sfxOn = true;
                private bool vibrateOn = true;
                private float musicVolume = 0f;
                private float sfxVolume = 0f;
                private float masterVolume = 0f;
                #endregion

                #region Unity Handle
                private void Start()
                {
                    if (option.HasFlag(OptionSetingSound.ToggleMusic))
                    {
                        musicOn = DataManager.Get_MusicOn();
                        SoundAndVibrateManager.Instance.musicSource.mute = !musicOn;
                        toggleMusic.Setup(musicOn);
                    }
                    if (option.HasFlag(OptionSetingSound.ToggleSFX))
                    {
                        sfxOn = DataManager.Get_SFXOn();
                        SoundAndVibrateManager.Instance.sfxSource.mute = !sfxOn;
                        toggleSFX.Setup(sfxOn);
                    }
                    if (option.HasFlag(OptionSetingSound.ToggleVibrate))
                    {
                        vibrateOn = DataManager.Get_VibrateOn();
                        SoundAndVibrateManager.Instance.vibrate = vibrateOn;
                        toggleVibrate.Setup(vibrateOn);
                    }
                    if (option.HasFlag(OptionSetingSound.SliderMusic))
                    {
                        musicVolume = DataManager.Get_MusicVolume();
                        masterVolume = DataManager.Get_MasterVolume();
                        sliderMusic.value = musicVolume;
                        SoundAndVibrateManager.Instance.musicSource.volume = musicVolume * masterVolume;
                        sliderMusic.onValueChanged.AddListener(val => UpdateMusicVolume(val));
                    }
                    if (option.HasFlag(OptionSetingSound.SliderSFX))
                    {
                        sfxVolume = DataManager.Get_SFXVolume();
                        masterVolume = DataManager.Get_MasterVolume();
                        sliderSFX.value = sfxVolume;
                        SoundAndVibrateManager.Instance.sfxSource.volume = sfxVolume * masterVolume;
                        sliderSFX.onValueChanged.AddListener(val => UpdateSFXVolume(val));
                    }
                    if (option.HasFlag(OptionSetingSound.SliderMaster))
                    {
                        masterVolume = DataManager.Get_MasterVolume();
                        sliderMaster.value = masterVolume;
                        SoundAndVibrateManager.Instance.musicSource.volume = musicVolume * masterVolume;
                        SoundAndVibrateManager.Instance.sfxSource.volume = sfxVolume * masterVolume;
                        sliderMaster.onValueChanged.AddListener(val => UpdateMasterVolume(val));
                    }
                }
                #endregion

                #region Sound Setting
                public void ToggleSFX()
                {
                    sfxOn = !sfxOn;
                    SoundAndVibrateManager.Instance.sfxSource.mute = !sfxOn;
                    DataManager.Set_SFXOn(sfxOn);
                }
                public void ToggleVibrate()
                {
                    vibrateOn = !vibrateOn;
                    SoundAndVibrateManager.Instance.vibrate = vibrateOn;
                    DataManager.Set_VibrateOn(vibrateOn);
                }
                public void ToggleMusic()
                {
                    musicOn = !musicOn;
                    SoundAndVibrateManager.Instance.musicSource.mute = !musicOn;
                    DataManager.Set_MusicOn(musicOn);
                }
                public void UpdateMusicVolume(float val)
                {
                    musicVolume = val;
                    SoundAndVibrateManager.Instance.musicSource.volume = musicVolume * masterVolume;
                    DataManager.Set_MusicVolume(musicVolume);
                }
                public void UpdateSFXVolume(float val)
                {
                    sfxVolume = val;
                    SoundAndVibrateManager.Instance.sfxSource.volume = sfxVolume * masterVolume;
                    DataManager.Set_SFXVolume(sfxVolume);
                }
                public void UpdateMasterVolume(float val)
                {
                    masterVolume = val;
                    SoundAndVibrateManager.Instance.musicSource.volume = musicVolume * masterVolume;
                    SoundAndVibrateManager.Instance.sfxSource.volume = sfxVolume * masterVolume;
                    DataManager.Set_MasterVolume(masterVolume);
                }
                #endregion
            }
        }
    }
}