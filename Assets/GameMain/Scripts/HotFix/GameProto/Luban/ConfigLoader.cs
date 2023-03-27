using System.Collections.Generic;
using System.IO;
using Bright.Serialization;
using GameConfig;
using SimpleJSON;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 配置加载器
/// </summary>
public class ConfigLoader:Singleton<ConfigLoader>
{
    private bool m_init = false;
    private Tables m_tables;
    public Tables Tables
    {
        get
        {
            if (!m_init)
            {
                m_init = true;
                Load();
            }
            return m_tables;
        }
    }

    private Dictionary<string, TextAsset> m_Configs = new Dictionary<string, TextAsset>();

    /// <summary>
    /// 注册配置资源。
    /// </summary>
    /// <param name="key">资源Key</param>
    /// <param name="value">资源TextAsset</param>
    /// <returns></returns>
    public bool RegisterTextAssets(string key, TextAsset value)
    {
        if (string.IsNullOrEmpty(key))
        {
            return false;
        }

        if (value == null)
        {
            return false;
        }
        m_Configs[key] = value;
        return true;
    }

    /// <summary>
    /// 加载配置
    /// </summary>
    public void Load()
    {
        var tablesCtor = typeof(Tables).GetConstructors()[0];
        var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];

        System.Delegate loader = loaderReturnType == typeof(ByteBuf)
            ? new System.Func<string, ByteBuf>(LoadByteBuf)
            : (System.Delegate)new System.Func<string, JSONNode>(LoadJson);
        m_tables = (Tables)tablesCtor.Invoke(new object[] { loader });
    }

    /// <summary>
    /// 加载Json配置
    /// </summary>
    /// <param name="file">FileName</param>
    /// <returns>JSONNode</returns>
    private JSONNode LoadJson(string file)
    {
        string ret = string.Empty;
#if UNITY_EDITOR
        ret = File.ReadAllText($"{Application.dataPath}/../GenerateDatas/json/{file}.json", System.Text.Encoding.UTF8);
#else
        var key = $"{file}.json";
        if (m_Configs.ContainsKey(key))
        {
            ret = m_Configs[key].text;
        }
        else
        {
            // var textAssets = GameModule.Resource.Load<TextAsset>($"{SettingsUtils.FrameworkGlobalSettings.ConfigFolderName}{file}.json");
            // ret = textAssets.text;
        }
#endif
        return JSON.Parse(ret);
    }

    /// <summary>
    /// 加载二进制配置
    /// </summary>
    /// <param name="file">FileName</param>
    /// <returns>ByteBuf</returns>
    private ByteBuf LoadByteBuf(string file)
    {
        byte[] ret = null;
#if UNITY_EDITOR
        ret = File.ReadAllBytes($"{Application.dataPath}/../GenerateDatas/bytes/{file}.bytes");
#else
        var key = $"{file}.bytes";
        if (m_Configs.ContainsKey(key))
        {
            ret = m_Configs[key].bytes;
        }
        else
        {
            // var textAssets = GameModule.Resource.Load<TextAsset>($"{SettingsUtils.FrameworkGlobalSettings.ConfigFolderName}{file}.bytes");
            // ret = textAssets.bytes;
        }
#endif
        return new ByteBuf(ret);
    }
}