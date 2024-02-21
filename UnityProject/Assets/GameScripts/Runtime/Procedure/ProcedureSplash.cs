using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    /// <summary>
    /// 流程 => 闪屏。
    /// </summary>
    public class ProcedureSplash : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            //播放 Splash 动画
            //热更新阶段文本初始化
            LoadText.Instance.InitConfigData(null);
            //热更新UI初始化
            UILoadMgr.Initialize();
            //初始化资源包
            ChangeState<ProcedureInitPackage>(procedureOwner);
        }
    }
}
