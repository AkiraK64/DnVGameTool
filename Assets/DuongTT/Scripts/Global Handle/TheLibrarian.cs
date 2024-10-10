using System.Collections;
using System.Collections.Generic;
using System;

// using for update money, gem... text everywhere

namespace DnVCorp
{
    namespace GlobalHandle
    {
        public static class TheLibrarian
        {
            static Dictionary<string, Dictionary<string, Action>> theBook = new Dictionary<string, Dictionary<string, Action>>();

            public static void Register(string mainTag, string extraTag, Action updateAction)
            {
                if (theBook.ContainsKey(mainTag))
                {
                    if (theBook[mainTag].ContainsKey(extraTag)) theBook[mainTag][extraTag] = updateAction;
                    else theBook[mainTag].Add(extraTag, updateAction);
                }
                else
                {
                    theBook.Add(mainTag, new Dictionary<string, Action>() { { extraTag, updateAction } });
                }
            }

            public static void UnRegister(string mainTag)
            {
                if (theBook.ContainsKey(mainTag)) theBook.Remove(mainTag);
            }

            public static void UnRegister(string mainTag, string extraTag)
            {
                if (theBook.ContainsKey(mainTag))
                {
                    if (theBook[mainTag].ContainsKey(extraTag))
                    {
                        theBook[mainTag].Remove(extraTag);
                    }
                }
            }

            public static void ClearAll()
            {
                theBook.Clear();
            }

            public static void Execute(string mainTag)
            {
                if (theBook.ContainsKey(mainTag))
                {
                    foreach(var action in theBook[mainTag].Values)
                    {
                        action?.Invoke();
                    }
                }
            }

            public static void Execute(string mainTag, string extraTag)
            {
                if (theBook.ContainsKey(mainTag))
                {
                    if (theBook[mainTag].ContainsKey(extraTag))
                    {
                        theBook[mainTag][extraTag]?.Invoke();
                    }
                }
            }

            public static void ExecuteAll()
            {
                foreach(var key in theBook.Keys)
                {
                    foreach (var action in theBook[key].Values)
                    {
                        action?.Invoke();
                    }
                }
            }
        }
    }
}
