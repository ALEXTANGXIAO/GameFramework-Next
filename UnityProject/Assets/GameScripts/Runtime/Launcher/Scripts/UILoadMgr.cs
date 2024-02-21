using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 热更界面加载管理器。
    /// </summary>
    public static class UILoadMgr
    {
        private static Transform _uiRoot;
        private static readonly Dictionary<string, string> _uiList = new Dictionary<string, string>();
        private static readonly Dictionary<string, UIBase> _uiMap = new Dictionary<string, UIBase>();

        /// <summary>
        /// 初始化根节点。
        /// </summary>
        public static void Initialize()
        {
            _uiRoot = GameModule.UI.UIRoot;
            if (_uiRoot == null)
            {
                Log.Error("Failed to Find UIRoot. Please check the resource path");
                return;
            }

            RegisterUI();
        }

        public static void RegisterUI()
        {
            UIDefine.RegisterUI(_uiList);
        }

        /// <summary>
        /// show ui
        /// </summary>
        /// <param name="uiInfo">对应的ui</param>
        /// <param name="param">参数</param>
        public static void Show(string uiInfo, object param = null)
        {
            if (string.IsNullOrEmpty(uiInfo))
            {
                return;
            }

            if (!_uiList.ContainsKey(uiInfo))
            {
                Log.Error($"not define ui:{uiInfo}");
                return;
            }

            GameObject ui = null;
            if (!_uiMap.ContainsKey(uiInfo))
            {
                Object obj = Resources.Load(_uiList[uiInfo]);
                if (obj != null)
                {
                    ui = Object.Instantiate(obj) as GameObject;
                    if (ui != null)
                    {
                        ui.transform.SetParent(_uiRoot.transform);
                        ui.transform.localScale = Vector3.one;
                        ui.transform.localPosition = Vector3.zero;
                        RectTransform rect = ui.GetComponent<RectTransform>();
                        rect.sizeDelta = Vector2.zero;
                    }
                }

                UIBase component = ui.GetComponent<UIBase>();
                if (component != null)
                {
                    _uiMap.Add(uiInfo, component);
                }
            }

            _uiMap[uiInfo].gameObject.SetActive(true);
            if (param != null)
            {
                UIBase component = _uiMap[uiInfo].GetComponent<UIBase>();
                if (component != null)
                {
                    component.OnEnter(param);
                }
            }
        }

        /// <summary>
        /// 隐藏ui对象
        /// </summary>
        /// <param name="uiName">对应的ui</param>
        public static void Hide(string uiName)
        {
            if (string.IsNullOrEmpty(uiName))
            {
                return;
            }

            if (!_uiMap.ContainsKey(uiName))
            {
                return;
            }

            _uiMap[uiName].gameObject.SetActive(false);
            Object.DestroyImmediate(_uiMap[uiName].gameObject);
            _uiMap.Remove(uiName);
        }

        /// <summary>
        /// 获取显示的ui对象
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static UIBase GetActiveUI(string ui)
        {
            return _uiMap.GetValueOrDefault(ui);
        }

        /// <summary>
        /// 隐藏所有热更相关UI。
        /// </summary>
        public static void HideAll()
        {
            foreach (var item in _uiMap)
            {
                if (item.Value && item.Value.gameObject)
                {
                    Object.Destroy(item.Value.gameObject);
                }
            }

            _uiMap.Clear();
        }
    }
}