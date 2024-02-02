using System;
using GameFramework;
using Newtonsoft.Json;


namespace GameMain
{
    public class NewtonsoftJsonHelper : Utility.Json.IJsonHelper
    {
        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object ToObject(Type objectType, string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }
}