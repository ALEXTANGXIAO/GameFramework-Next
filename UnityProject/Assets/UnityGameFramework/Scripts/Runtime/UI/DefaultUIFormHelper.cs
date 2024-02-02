using GameFramework.UI;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认界面辅助器。
    /// </summary>
    public class DefaultUIFormHelper : UIFormHelperBase
    {
        private ResourceComponent m_ResourceComponent = null;

        private Vector2 m_Half = new Vector2(0.5f,0.5f);

        private int m_UILayer;

        /// <summary>
        /// 实例化界面。
        /// </summary>
        /// <param name="uiFormAsset">要实例化的界面资源。</param>
        /// <returns>实例化后的界面。</returns>
        public override object InstantiateUIForm(object uiFormAsset)
        {
            return Instantiate((Object)uiFormAsset);
        }

        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        public override IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
        {
            GameObject obj = uiFormInstance as GameObject;
            if (obj == null)
            {
                Log.Error("UI form instance is invalid.");
                return null;
            }

            Transform trans = obj.transform;
            trans.SetParent(((MonoBehaviour)uiGroup.Helper).transform);
            trans.localScale = Vector3.one;
            trans.localPosition = Vector3.zero;
            obj.layer = m_UILayer;
            
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchorMin = m_Half;
            rectTransform.anchorMax = m_Half;
            rectTransform.anchoredPosition = Vector2.zero;
            return obj.GetOrAddComponent<UIForm>();
        }

        /// <summary>
        /// 释放界面。
        /// </summary>
        /// <param name="uiFormAsset">要释放的界面资源。</param>
        /// <param name="uiFormInstance">要释放的界面实例。</param>
        public override void ReleaseUIForm(object uiFormAsset, object uiFormInstance)
        {
            m_ResourceComponent.UnloadAsset(uiFormAsset);
            Destroy((Object)uiFormInstance);
        }

        private void Start()
        {
            m_UILayer = LayerMask.NameToLayer("UI");
            m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}
