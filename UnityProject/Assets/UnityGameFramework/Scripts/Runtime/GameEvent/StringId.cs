using System.Collections.Generic;

namespace UnityGameFramework.Runtime
{
    public class StringId
    {
        private static readonly Dictionary<string, int> s_EventTypeHashMap = new Dictionary<string, int>();
        private static readonly Dictionary<int, string> s_EventHashToStringMap = new Dictionary<int, string>();
        private static int s_CurrentId = 0;

        public static int StringToHash(string val)
        {
            if (s_EventTypeHashMap.TryGetValue(val, out var hashId))
            {
                return hashId;
            }

            hashId = ++s_CurrentId;
            s_EventTypeHashMap[val] = hashId;
            s_EventHashToStringMap[hashId] = val;

            return hashId;
        }

        public static string HashToString(int hash)
        {
            if (s_EventHashToStringMap.TryGetValue(hash, out var value))
            {
                return value;
            }
            return string.Empty;
        }
    }
}