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
            GameObject formInstance = uiFormInstance as GameObject;
            if (formInstance == null)
            {
                Log.Error("UI form instance is invalid.");
                return null;
            }

            RectTransform rectTransform = formInstance.transform as RectTransform;
            Vector3 localPosition = rectTransform.localPosition;
            Vector3 localScale = rectTransform.localScale;
            Vector3 eulerAngles = rectTransform.localEulerAngles;
            Vector3 sizeDelta = rectTransform.sizeDelta;
            Vector3 anchorMin = rectTransform.anchorMin;
            Vector3 anchorMax = rectTransform.anchorMax;
            Vector3 anchoredPosition = rectTransform.anchoredPosition;
            Vector3 offsetMin = rectTransform.offsetMin;
            Vector3 offsetMax = rectTransform.offsetMax;
            Vector3 pivot = rectTransform.pivot;
            
            rectTransform.SetParent(((MonoBehaviour)uiGroup.Helper).transform);

            rectTransform.localPosition = localPosition;
            rectTransform.localScale = localScale;
            rectTransform.eulerAngles = eulerAngles;
            rectTransform.sizeDelta = sizeDelta;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
            rectTransform.pivot = pivot;

            return formInstance.GetOrAddComponent<UIForm>();
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
            m_ResourceComponent = GameSystem.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}
