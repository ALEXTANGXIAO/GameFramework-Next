using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class JsonLocalizationHelper : DefaultLocalizationHelper
    {
        public struct LanguageKeyValue
        {
            public string Key;
            public string Value;
        }

        /// <summary>
        /// 解析多语言json
        /// </summary>
        /// <param name="localizationManager"></param>
        /// <param name="rawString"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public override bool ParseData(ILocalizationManager localizationManager, string rawString, object userData)
        {
            try
            {
                string currentLanguage = GameModule.Localization.Language.ToString();
                List<LanguageKeyValue> languageKeyValues = Utility.Json.ToObject<List<LanguageKeyValue>>(rawString);
                for (int i = 0; i < languageKeyValues.Count; i++)
                {
                    var json = languageKeyValues[i];
                    string key = json.Key;
                    string value = json.Value;
                    if (!localizationManager.AddRawString(key, value))
                    {
                        Log.Warning("Can not add raw string with key '{0}' which may be invalid or duplicate.", key);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse dictionary data with exception '{0}'.", exception.ToString());
                return false;
            }
        }
    }
}