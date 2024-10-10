using System;
using UnityEngine;
using UnityEngine.UI;

namespace DnVCorp
{
    namespace Systems
    {
        [Serializable]
        public struct TimeStruct
        {
            public int Year;
            public int Month;
            public int Day;
            public int Hour;
            public int Minute;
            public int Second;

            public TimeStruct(int _Year, int _Month, int _Day, int _Hour, int _Minute, int _Second)
            {
                Year = _Year;
                Month = _Month;
                Day = _Day;
                Hour = _Hour;
                Minute = _Minute;
                Second = _Second;
            }

            public TimeStruct(DateTime day)
            {
                Year = day.Year;
                Month = day.Month;
                Day = day.Day;
                Hour = day.Hour;
                Minute = day.Minute;
                Second = day.Second;
            }

            public bool IsNewDay(TimeStruct checkPointDay)
            {
                if (Year < checkPointDay.Year) return false;
                if (Year > checkPointDay.Year) return true;
                if (Month < checkPointDay.Month) return false;
                if (Month > checkPointDay.Month) return true;
                if (Day <= checkPointDay.Day) return false;
                return true;
            }
        }      

        [Serializable]
        public struct ResourceInGameStruct
        {
            public Sprite icon;
            public int value;
            public bool special;
            public Action<int, int> applyAction;

            public ResourceInGameStruct(Sprite _icon, int _value, bool _special, Action<int, int> _applyAction)
            {
                icon = _icon;
                value = _value;
                special = _special;
                applyAction = _applyAction;
            }
        }

        [Serializable]
        public struct InteractableSpriteStruct
        {
            public Image sourceImage;
            public Sprite onSprite;
            public Sprite offSprite;
        }

        [Serializable]
        public struct PairStruct<T0, T1>
        {
            public T0 Key;
            public T1 Value;

            public PairStruct(T0 _key, T1 _value)
            {
                Key = _key;
                Value = _value;
            }
        }

        [Serializable]
        public struct TrioStruct<T0, T1, T2>
        {
            public T0 Item1;
            public T1 Item2;
            public T2 Item3;

            public TrioStruct(T0 _item1, T1 _item2, T2 _item3)
            {
                Item1 = _item1;
                Item2 = _item2;
                Item3 = _item3;
            }
        }

        [Serializable]
        public struct QuartetStruct<T0, T1, T2, T3>
        {
            public T0 Item1;
            public T1 Item2;
            public T2 Item3;
            public T3 Item4;

            public QuartetStruct(T0 _item1, T1 _item2, T2 _item3, T3 _item4)
            {
                Item1 = _item1;
                Item2 = _item2;
                Item3 = _item3;
                Item4 = _item4;
            }
        }
    }
}
