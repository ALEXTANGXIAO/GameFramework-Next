using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityGameFramework.Editor;
using UnityGameFramework.Runtime;

namespace YooAsset.Editor.Inspector
{
    [CustomEditor(typeof(ResourceComponent))]
    internal sealed class ResourceComponentInspector : GameFrameworkInspector
    {
        private static readonly string[] ResourceModeNames = new string[] { "EditorSimulateMode (编辑器下的模拟模式)", "OfflinePlayMode (单机模式)", "HostPlayMode (联机运行模式)" };
        private static readonly string[] VerifyLevelNames = new string[] { "Low (验证文件存在)", "Middle (验证文件大小)", "High (验证文件大小和CRC)" };
        
        private SerializedProperty m_PackageName = null;
        private SerializedProperty m_PlayMode = null;
        private SerializedProperty m_ReadWritePathType = null;
        private SerializedProperty m_VerifyLevel = null;
        private SerializedProperty m_Milliseconds = null;
        private SerializedProperty m_MinUnloadUnusedAssetsInterval = null;
        private SerializedProperty m_MaxUnloadUnusedAssetsInterval = null;
        private SerializedProperty m_DownloadingMaxNum = null;
        private SerializedProperty m_FailedTryAgain = null;
        
        private int m_ResourceModeIndex = 0;
        private int m_PackageIndex = 0;
        private int m_VerifyIndex = 0;
        
        private PopupField<string> _buildPackageField;
        private List<string> _buildPackageNames;
        
        private void OnEnable()
        {
            m_PackageName = serializedObject.FindProperty("PackageName");
            m_PlayMode = serializedObject.FindProperty("PlayMode");
            m_VerifyLevel = serializedObject.FindProperty("VerifyLevel");
            m_Milliseconds = serializedObject.FindProperty("Milliseconds");

            m_ReadWritePathType = serializedObject.FindProperty("m_ReadWritePathType");
            m_MinUnloadUnusedAssetsInterval = serializedObject.FindProperty("m_MinUnloadUnusedAssetsInterval");
            m_MaxUnloadUnusedAssetsInterval = serializedObject.FindProperty("m_MaxUnloadUnusedAssetsInterval");
            m_DownloadingMaxNum = serializedObject.FindProperty("m_DownloadingMaxNum");
            m_FailedTryAgain = serializedObject.FindProperty("m_FailedTryAgain");
            
            RefreshModes();
            RefreshTypeNames();
        }
        
        private void RefreshModes()
        {
            m_ResourceModeIndex = m_PlayMode.enumValueIndex > 0 ? m_PlayMode.enumValueIndex: 0;
            m_VerifyIndex = m_VerifyLevel.enumValueIndex > 0 ? m_VerifyLevel.enumValueIndex: 0;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            ResourceComponent t = (ResourceComponent)target;
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
                {
                    EditorGUILayout.EnumPopup("Resource Mode", t.PlayMode);
                    
                    EditorGUILayout.EnumPopup("VerifyLevel", t.VerifyLevel);
                    
                    _buildPackageNames = GetBuildPackageNames();
                    if (_buildPackageNames.Count > 0)
                    {
                        GUILayout.Label(_buildPackageNames[0]);
                    }
                }
                else
                {
                    // 资源模式
                    int selectedIndex = EditorGUILayout.Popup("Resource Mode", m_ResourceModeIndex, ResourceModeNames);
                    if (selectedIndex != m_ResourceModeIndex)
                    {
                        m_ResourceModeIndex = selectedIndex;
                        m_PlayMode.enumValueIndex = selectedIndex;
                    }
                    
                    int selectedVerifyIndex = EditorGUILayout.Popup("VerifyLevel", m_VerifyIndex, VerifyLevelNames);
                    if (selectedVerifyIndex != m_VerifyIndex)
                    {
                        m_VerifyIndex = selectedVerifyIndex;
                        m_VerifyLevel.enumValueIndex = selectedVerifyIndex;
                    }
                    
                    // 包裹名称列表
                    _buildPackageNames = GetBuildPackageNames();
                    if (_buildPackageNames.Count > 0)
                    {
                        int selectedPackageIndex = EditorGUILayout.Popup("Used Packages", m_PackageIndex, _buildPackageNames.ToArray());
                        if (selectedPackageIndex != m_PackageIndex)
                        {
                            m_PackageIndex = selectedPackageIndex;
                            m_PlayMode.enumValueIndex = selectedIndex + 1;
                        }
                    
                        int defaultIndex = GetDefaultPackageIndex(AssetBundleBuilderSettingData.Setting.BuildPackage);
                        _buildPackageField = new PopupField<string>(_buildPackageNames, defaultIndex);
                        _buildPackageField.label = "Build Package";
                        _buildPackageField.style.width = 350;
                        _buildPackageField.RegisterValueChangedCallback(evt =>
                        {
                            AssetBundleBuilderSettingData.IsDirty = true;
                            AssetBundleBuilderSettingData.Setting.BuildPackage = _buildPackageField.value;
                        });
                    }
                    else
                    {
                        GUILayout.Label("Please Create Packages with YooAssets ...!");
                    }
                }
                m_ReadWritePathType.enumValueIndex = (int)(ReadWritePathType)EditorGUILayout.EnumPopup("Read-Write Path Type", t.ReadWritePathType);
            }
            EditorGUI.EndDisabledGroup();
            
            int milliseconds = EditorGUILayout.DelayedIntField("Milliseconds", m_Milliseconds.intValue);
            if (milliseconds != m_Milliseconds.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.Milliseconds = milliseconds;
                }
                else
                {
                    m_Milliseconds.longValue = milliseconds;
                }
            }
            
            float minUnloadUnusedAssetsInterval = EditorGUILayout.Slider("Min Unload Unused Assets Interval", m_MinUnloadUnusedAssetsInterval.floatValue, 0f, 3600f);
            if (Math.Abs(minUnloadUnusedAssetsInterval - m_MinUnloadUnusedAssetsInterval.floatValue) > 0.001f)
            {
                if (EditorApplication.isPlaying)
                {
                    t.MinUnloadUnusedAssetsInterval = minUnloadUnusedAssetsInterval;
                }
                else
                {
                    m_MinUnloadUnusedAssetsInterval.floatValue = minUnloadUnusedAssetsInterval;
                }
            }

            float maxUnloadUnusedAssetsInterval = EditorGUILayout.Slider("Max Unload Unused Assets Interval", m_MaxUnloadUnusedAssetsInterval.floatValue, 0f, 3600f);
            if (Math.Abs(maxUnloadUnusedAssetsInterval - m_MaxUnloadUnusedAssetsInterval.floatValue) > 0.001f)
            {
                if (EditorApplication.isPlaying)
                {
                    t.MaxUnloadUnusedAssetsInterval = maxUnloadUnusedAssetsInterval;
                }
                else
                {
                    m_MaxUnloadUnusedAssetsInterval.floatValue = maxUnloadUnusedAssetsInterval;
                }
            }
            
            float downloadingMaxNum = EditorGUILayout.Slider("Max Downloading Num", m_DownloadingMaxNum.intValue, 1f, 48f);
            if (Math.Abs(downloadingMaxNum - m_DownloadingMaxNum.intValue) > 0.001f)
            {
                if (EditorApplication.isPlaying)
                {
                    t.m_DownloadingMaxNum = (int)downloadingMaxNum;
                }
                else
                {
                    m_DownloadingMaxNum.intValue = (int)downloadingMaxNum;
                }
            }
            
            float failedTryAgain = EditorGUILayout.Slider("Max FailedTryAgain Count", m_FailedTryAgain.intValue, 1f, 48f);
            if (Math.Abs(failedTryAgain - m_FailedTryAgain.intValue) > 0.001f)
            {
                if (EditorApplication.isPlaying)
                {
                    t.m_FailedTryAgain = (int)failedTryAgain;
                }
                else
                {
                    m_FailedTryAgain.intValue = (int)failedTryAgain;
                }
            }

            
            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Unload Unused Assets", Utility.Text.Format("{0:F2} / {1:F2}", t.LastUnloadUnusedAssetsOperationElapseSeconds, t.MaxUnloadUnusedAssetsInterval));
                EditorGUILayout.LabelField("Read-Only Path", t.ReadOnlyPath.ToString());
                EditorGUILayout.LabelField("Read-Write Path", t.ReadWritePath.ToString());
                EditorGUILayout.LabelField("Applicable Game Version", t.ApplicableGameVersion ?? "<Unknwon>");
                EditorGUILayout.LabelField("Internal Resource Version",t.InternalResourceVersion.ToString());
                // EditorGUILayout.LabelField("Asset Count", isEditorResourceMode ? "N/A" : t.AssetCount.ToString());
                // EditorGUILayout.LabelField("Resource Count", isEditorResourceMode ? "N/A" : t.ResourceCount.ToString());
                // EditorGUILayout.LabelField("Resource Group Count", isEditorResourceMode ? "N/A" : t.ResourceGroupCount.ToString());
                // if (m_ResourceModeIndex > 0)
                // {
                //     EditorGUILayout.LabelField("Applying Resource Pack Path", isEditorResourceMode ? "N/A" : t.ApplyingResourcePackPath ?? "<Unknwon>");
                //     EditorGUILayout.LabelField("Apply Waiting Count", isEditorResourceMode ? "N/A" : t.ApplyWaitingCount.ToString());
                //     EditorGUILayout.LabelField("Updating Resource Group", isEditorResourceMode ? "N/A" : t.UpdatingResourceGroup != null ? t.UpdatingResourceGroup.Name : "<Unknwon>");
                //     EditorGUILayout.LabelField("Update Waiting Count", isEditorResourceMode ? "N/A" : t.UpdateWaitingCount.ToString());
                //     EditorGUILayout.LabelField("Update Waiting While Playing Count", isEditorResourceMode ? "N/A" : t.UpdateWaitingWhilePlayingCount.ToString());
                //     EditorGUILayout.LabelField("Update Candidate Count", isEditorResourceMode ? "N/A" : t.UpdateCandidateCount.ToString());
                // }
                // EditorGUILayout.LabelField("Load Total Agent Count", isEditorResourceMode ? "N/A" : t.LoadTotalAgentCount.ToString());
                // EditorGUILayout.LabelField("Load Free Agent Count", isEditorResourceMode ? "N/A" : t.LoadFreeAgentCount.ToString());
                // EditorGUILayout.LabelField("Load Working Agent Count", isEditorResourceMode ? "N/A" : t.LoadWorkingAgentCount.ToString());
                // EditorGUILayout.LabelField("Load Waiting Task Count", isEditorResourceMode ? "N/A" : t.LoadWaitingTaskCount.ToString());
                // if (!isEditorResourceMode)
                // {
                //     EditorGUILayout.BeginVertical("box");
                //     {
                //         TaskInfo[] loadAssetInfos = t.GetAllLoadAssetInfos();
                //         if (loadAssetInfos.Length > 0)
                //         {
                //             foreach (TaskInfo loadAssetInfo in loadAssetInfos)
                //             {
                //                 DrawLoadAssetInfo(loadAssetInfo);
                //             }
                //
                //             if (GUILayout.Button("Export CSV Data"))
                //             {
                //                 string exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty, "Load Asset Task Data.csv", string.Empty);
                //                 if (!string.IsNullOrEmpty(exportFileName))
                //                 {
                //                     try
                //                     {
                //                         int index = 0;
                //                         string[] data = new string[loadAssetInfos.Length + 1];
                //                         data[index++] = "Load Asset Name,Serial Id,Priority,Status";
                //                         foreach (TaskInfo loadAssetInfo in loadAssetInfos)
                //                         {
                //                             data[index++] = Utility.Text.Format("{0},{1},{2},{3}", loadAssetInfo.Description, loadAssetInfo.SerialId, loadAssetInfo.Priority, loadAssetInfo.Status);
                //                         }
                //
                //                         File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                //                         Debug.Log(Utility.Text.Format("Export load asset task CSV data to '{0}' success.", exportFileName));
                //                     }
                //                     catch (Exception exception)
                //                     {
                //                         Debug.LogError(Utility.Text.Format("Export load asset task CSV data to '{0}' failure, exception is '{1}'.", exportFileName, exception));
                //                     }
                //                 }
                //             }
                //         }
                //         else
                //         {
                //             GUILayout.Label("Load Asset Task is Empty ...");
                //         }
                //     }
                    // EditorGUILayout.EndVertical();
                // }
            }
            
            serializedObject.ApplyModifiedProperties();

            Repaint();
        }
        
        private void DrawLoadAssetInfo(TaskInfo loadAssetInfo)
        {
            EditorGUILayout.LabelField(loadAssetInfo.Description, Utility.Text.Format("[SerialId]{0} [Priority]{1} [Status]{2}", loadAssetInfo.SerialId, loadAssetInfo.Priority, loadAssetInfo.Status));
        }
        
        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }
        
        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }
        
        // 构建包裹相关
        private int GetDefaultPackageIndex(string packageName)
        {
            for (int index = 0; index < _buildPackageNames.Count; index++)
            {
                if (_buildPackageNames[index] == packageName)
                {
                    return index;
                }
            }

            AssetBundleBuilderSettingData.IsDirty = true;
            AssetBundleBuilderSettingData.Setting.BuildPackage = _buildPackageNames[0];
            return 0;
        }
        
        private List<string> GetBuildPackageNames()
        {
            List<string> result = new List<string>();
            foreach (var package in AssetBundleCollectorSettingData.Setting.Packages)
            {
                result.Add(package.PackageName);
            }
            return result;
        }
    }
}