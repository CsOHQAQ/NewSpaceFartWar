using QxFramework.Core;

namespace SaveFramework
{
    public class SaveFrameProcedure : ProcedureBase
    {
        protected override void OnEnter(object args)
        {
            base.OnEnter(args);
            QXData.Instance.SetTableAgent();
            GameMgr.Instance.InitModules();
            UIManager.Instance.Open("Example_SaveTestUI");
        }
    }
}