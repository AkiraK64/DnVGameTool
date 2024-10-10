using DnVCorp.Systems;

namespace DnVCorp
{
    namespace Manager
    {
        public static class DataManager
        {
            #region Sound And Vibration Keys
            static string SOUND_MUSIC_ON = "SOUND_MUSIC_ON";
            static string SOUND_MUSIC_VOLUME = "SOUND_MUSIC_VOLUME";
            static string SOUND_SFX_ON = "SOUND_SFX_ON";
            static string SOUND_SFX_VOLUME = "SOUND_SFX_VOLUME";
            static string SOUND_MASTER_VOLUME = "SOUND_MASTER_VOLUME";
            static string VIBRATION_TOGGLE = "VIBRATION_TOGGLE";
            #endregion

            #region Economic Keys
            static string RESOURCE_MONEY = "RESOURCE_MONEY";
            static string RESOURCE_TICKET = "RESOURCE_TICKET";
            static string RESOURCE_KEY = "RESOURCE_KEY";
            static string RESOURCE_GEM = "RESOURCE_GEM";
            #endregion

            #region Sound And Vibration Handle
            public static bool Get_MusicOn()
            {
                return NewPlayerPrefs.GetBool(SOUND_MUSIC_ON, true);
            }
            public static void Set_MusicOn(bool isTrue)
            {
                NewPlayerPrefs.SetBool(SOUND_MUSIC_ON, isTrue);
            }
            public static bool Get_SFXOn()
            {
                return NewPlayerPrefs.GetBool(SOUND_SFX_ON, true);
            }
            public static void Set_SFXOn(bool isTrue)
            {
                NewPlayerPrefs.SetBool(SOUND_SFX_ON, isTrue);
            }
            public static float Get_MusicVolume()
            {
                return NewPlayerPrefs.GetFloat(SOUND_MUSIC_VOLUME, 1f);
            }
            public static void Set_MusicVolume(float value)
            {
                NewPlayerPrefs.SetFloat(SOUND_MUSIC_VOLUME, value);
            }
            public static float Get_SFXVolume()
            {
                return NewPlayerPrefs.GetFloat(SOUND_SFX_VOLUME, 1f);
            }
            public static void Set_SFXVolume(float value)
            {
                NewPlayerPrefs.SetFloat(SOUND_SFX_VOLUME, value);
            }
            public static float Get_MasterVolume()
            {
                return NewPlayerPrefs.GetFloat(SOUND_MASTER_VOLUME, 1f);
            }
            public static void Set_MasterVolume(float value)
            {
                NewPlayerPrefs.SetFloat(SOUND_MASTER_VOLUME, value);
            }
            public static bool Get_VibrateOn()
            {
                return NewPlayerPrefs.GetBool(VIBRATION_TOGGLE, true);
            }
            public static void Set_VibrateOn(bool isTrue)
            {
                NewPlayerPrefs.SetBool(VIBRATION_TOGGLE, isTrue);
            }
            #endregion

            #region Money Data Handle
            public static int Get_Money()
            {
                return NewPlayerPrefs.GetInt(RESOURCE_MONEY, 0);
            }
            public static void Set_Money(int value)
            {
                NewPlayerPrefs.SetInt(RESOURCE_MONEY, value);
            }
            public static void Earn_Money(int value)
            {
                Set_Money(Get_Money() + value);
            }
            public static bool Enough_Money(int value)
            {
                return Get_Money() >= value;
            }
            public static void Spend_Gold(int value)
            {
                if (Get_Money() >= value)
                {
                    Set_Money(Get_Money() - value);
                }
            }
            #endregion

            #region Ticket Data Handle
            public static int Get_Ticket()
            {
                return NewPlayerPrefs.GetInt(RESOURCE_TICKET, 0);
            }
            public static void Set_Ticket(int value)
            {
                NewPlayerPrefs.SetInt(RESOURCE_TICKET, value);
            }
            public static void Earn_Ticket(int value)
            {
                Set_Ticket(Get_Ticket() + value);
            }
            public static bool Enough_Ticket(int value)
            {
                return Get_Ticket() >= value;
            }
            public static void Spend_Ticket(int value)
            {
                if (Get_Ticket() >= value)
                {
                    Set_Ticket(Get_Ticket() - value);
                }
            }
            #endregion

            #region Key Data Handle
            public static int Get_Key()
            {
                return NewPlayerPrefs.GetInt(RESOURCE_KEY, 0);
            }
            public static void Set_Key(int value)
            {
                NewPlayerPrefs.SetInt(RESOURCE_KEY, value);
            }
            public static void Earn_Key(int value)
            {
                Set_Key(Get_Key() + value);
            }
            public static bool Enough_Key(int value)
            {
                return Get_Key() >= value;
            }
            public static void Spend_Key(int value)
            {
                if (Get_Key() >= value)
                {
                    Set_Key(Get_Key() - value);
                }
            }
            #endregion

            #region Gem Data Handle
            public static int Get_Gem()
            {
                return NewPlayerPrefs.GetInt(RESOURCE_GEM, 0);
            }
            public static void Set_Gem(int value)
            {
                NewPlayerPrefs.SetInt(RESOURCE_GEM, value);
            }
            public static void Earn_Gem(int value)
            {
                Set_Gem(Get_Gem() + value);
            }
            public static bool Enough_Gem(int value)
            {
                return Get_Gem() >= value;
            }
            public static void Spend_Gem(int value)
            {
                if (Get_Gem() >= value)
                {
                    Set_Gem(Get_Gem() - value);
                }
            }
            #endregion

        }
    }
}