using GameFramework.Event;
using System.Collections.Generic;
using System.Xml;
using GameFramework;
using GameFramework.Resource;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    /// <summary>
    /// 预加载流程
    /// </summary>
    public class ProcedurePreload : ProcedureBase
    {
        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

        public override bool UseNativeDialog
        {
            get { return true; }
        }

        private bool m_needProLoadConfig = false;

        private bool m_InitConfigXml = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameModule.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameModule.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameModule.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameModule.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameModule.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameModule.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            m_LoadedFlag.Clear();

            if (GameModule.Resource.PlayMode == EPlayMode.EditorSimulateMode)
            {
                m_InitConfigXml = true;
            }

            UILoadMgr.Show(UIDefine.UILoadUpdate, Utility.Text.Format(LoadText.Instance.Label_Load_Load_Progress, 0));

            PreloadResources();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameModule.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameModule.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameModule.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameModule.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameModule.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameModule.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            var totalCount = m_LoadedFlag.Count <= 0 ? 1 : m_LoadedFlag.Count;

            var loadCount = m_LoadedFlag.Count <= 0 ? 1 : 0;

            foreach (KeyValuePair<string, bool> loadedFlag in m_LoadedFlag)
            {
                if (!loadedFlag.Value)
                {
                    break;
                }
                else
                {
                    loadCount++;
                }
            }

            UILoadMgr.Show(UIDefine.UILoadUpdate, Utility.Text.Format(LoadText.Instance.Label_Load_Load_Progress, (float)loadCount / totalCount * 100));

            if (loadCount < totalCount)
            {
                return;
            }

            if (m_InitConfigXml == false)
            {
                return;
            }

            UILoadMgr.HideAll();

            ChangeState<ProcedureLoadAssembly>(procedureOwner);
        }

        private void PreloadResources()
        {
            AwaitableExtensions.SubscribeEvent();
            if (m_needProLoadConfig)
            {
                LoadAllConfig();
            }
            else
            {
                m_InitConfigXml = true;
            }
        }

        private void LoadAllConfig()
        {
            if (GameModule.Resource.PlayMode == EPlayMode.EditorSimulateMode)
            {
                m_InitConfigXml = true;
                return;
            }
        }

        private void LoadConfig(string configName)
        {
            string configAssetName = SettingsUtils.GetConfigAsset(configName);
            m_LoadedFlag.Add(configAssetName, false);
            GameModule.Config.ReadData(configAssetName, this);
        }

        private void LoadDictionary(string dictionaryName)
        {
            string dictionaryAssetName = SettingsUtils.GetDictionaryAsset(dictionaryName, false);
            m_LoadedFlag.Add(dictionaryAssetName, false);
            GameModule.Localization.ReadData(dictionaryAssetName, this);
        }

        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.ConfigAssetName] = true;
            Log.Info("Load config '{0}' OK.", ne.ConfigAssetName);
        }

        private void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigAssetName,
                ne.ConfigAssetName, ne.ErrorMessage);
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.DataTableAssetName] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableAssetName);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableAssetName,
                ne.DataTableAssetName, ne.ErrorMessage);
        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.DictionaryAssetName] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryAssetName);
        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryAssetName,
                ne.DictionaryAssetName, ne.ErrorMessage);
        }
    }
}