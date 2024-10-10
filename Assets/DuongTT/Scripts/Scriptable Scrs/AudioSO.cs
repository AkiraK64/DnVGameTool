using DnVCorp.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DnVCorp
{
    namespace Datas
    {
        [CreateAssetMenu(menuName = "AudioAssetSO", fileName = "New AudioAsset")]
        public class AudioSO : ScriptableObject
        {
            [Title("Default Clip")]
            public AudioClip defaultClip;
          
            [Title("List Clips")]
            [TableList]
            [ReadOnly]
            public List<PairStruct<string, AudioClip>> theList = new List<PairStruct<string, AudioClip>>();

            Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

            [Title("Handle Clip")]
            public string nameOfClip;
            public AudioClip audioClip;

            [TitleGroup("Action")]
            [ButtonGroup("Action/Button", ButtonHeight = 50)]
            //[InfoBox("Add new element or change exist element")]
            public void Add()
            {
#if UNITY_EDITOR
                if (audioDict.ContainsKey(nameOfClip))
                {
                    int index = theList.FindIndex(x => x.Key == nameOfClip);
                    theList[index] = new PairStruct<string, AudioClip>(nameOfClip, audioClip);
                    audioDict[nameOfClip] = audioClip;
                }
                else
                {
                    theList.Add(new PairStruct<string, AudioClip>(nameOfClip, audioClip));
                    audioDict.Add(nameOfClip, audioClip);
                    nameOfClip = "";
                    audioClip = null;
                }
#endif
            }
  
            [TitleGroup("Action")]
            [ButtonGroup("Action/Button", ButtonHeight = 50)]
            //[InfoBox("Remove one element by name of clip")]
            public void Remove()
            {
#if UNITY_EDITOR
                if (!audioDict.ContainsKey(nameOfClip)) return;
                theList.Remove(theList.Find(x => x.Key == nameOfClip));
                audioDict.Remove(nameOfClip);
                nameOfClip = "";
                audioClip = null;
#endif
            }

            [TitleGroup("Action")]
            [ButtonGroup("Action/Button", ButtonHeight = 50)]
            //[InfoBox("Sort the list")]
            public void Sort()
            {
#if UNITY_EDITOR
                theList = theList.OrderBy(p => p.Key).ToList();
#endif
            }

            [TitleGroup("Action")]
            [ButtonGroup("Action/Button", ButtonHeight = 50)]
            //[InfoBox("Clear list")]
            public void Clear()
            {
#if UNITY_EDITOR
                theList = new List<PairStruct<string, AudioClip>>();
                audioDict = new Dictionary<string, AudioClip>();
#endif
            }

            #region Behavior
            public Dictionary<string, AudioClip> GenerateDict()
            {
                Dictionary<string, AudioClip> dict = new Dictionary<string, AudioClip>();
                foreach(var element in theList)
                {
                    if (dict.ContainsKey(element.Key)) dict[element.Key] = element.Value;
                    else dict.Add(element.Key, element.Value);
                }
                return dict;
            }            
            #endregion
        }
    }
}
