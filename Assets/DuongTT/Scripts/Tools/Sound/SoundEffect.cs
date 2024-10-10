using DnVCorp;
using DnVCorp.Manager;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnVCorp
{
    namespace Tools
    {
        namespace Sound
        {
            [RequireComponent(typeof(AudioSource))]
            public class SoundEffect : MonoBehaviour
            {
                [PropertySpace(SpaceBefore = 10f, SpaceAfter = 0f)]
                [Title("Effect Source", null, TitleAlignments.Centered)]
                public AudioSource source;
                [Range(0f, 1f)]
                public float baseVolume = 1f;

                #region TMP Variables
                private float masterVolume = 1f;
                private float sfxVolume = 1f;
                private bool sfxOn = true;
                private bool playing = false;                
                #endregion

                public void Play()
                {
                    sfxOn = DataManager.Get_SFXOn();                   
                    if (sfxOn)
                    {
                        sfxVolume = DataManager.Get_SFXVolume();
                        masterVolume = DataManager.Get_MasterVolume();
                        source.volume = sfxVolume * masterVolume * baseVolume;
                        source.Play();
                        playing = true;
                    }
                }
                public void Stop()
                {
                    if (playing)
                    {
                        source.Stop();
                        playing = false;
                    }
                }
            }
        }
    }
}
