using System;
using System.Collections.Generic;
using System.Linq;


public static class SystemCollectionsExtension
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
        foreach (var item in self)
        {
            action(item);
        }

        return self;
    }

    public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T> action)
    {
        for (var i = selfList.Count - 1; i >= 0; --i)
        {
            action(selfList[i]);
        }

        return selfList;
    }

    public static void ForEach<T>(this List<T> list, Action<int, T> action)
    {
        for (var i = 0; i < list.Count; i++)
        {
            action(i, list[i]);
        }
    }

    public static void ForEach<K, V>(this Dictionary<K, V> dict, Action<K, V> action)
    {
        var dictE = dict.GetEnumerator();

        while (dictE.MoveNext())
        {
            var current = dictE.Current;
            action(current.Key, current.Value);
        }

        dictE.Dispose();
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        params Dictionary<TKey, TValue>[] dictionaries)
    {
        return dictionaries.Aggregate(dictionary,
            (current, dict) => current.Union(dict).ToDictionary(kv => kv.Key, kv => kv.Value));
    }

    public static void AddRange<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> addInDict,
        bool isOverride = false)
    {
        var enumerator = addInDict.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            if (dict.ContainsKey(current.Key))
            {
                if (isOverride)
                    dict[current.Key] = current.Value;
                continue;
            }

            dict.Add(current.Key, current.Value);
        }

        enumerator.Dispose();
    }


    public static bool IsNullOrEmpty<T>(this T[] collection) => collection == null || collection.Length == 0;

    public static bool IsNullOrEmpty<T>(this IList<T> collection) => collection == null || collection.Count == 0;

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();

    public static bool IsNotNullAndEmpty<T>(this T[] collection) => !IsNullOrEmpty(collection);

    public static bool IsNotNullAndEmpty<T>(this IList<T> collection) => !IsNullOrEmpty(collection);

    public static bool IsNotNullAndEmpty<T>(this IEnumerable<T> collection) => !IsNullOrEmpty(collection);
}