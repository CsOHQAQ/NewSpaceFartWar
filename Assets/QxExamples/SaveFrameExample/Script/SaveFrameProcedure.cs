using QxFramework.Core;

namespace SaveFramework
{
    public class SaveFrameProcedure : ProcedureBase
    {
        protected override void OnEnter(object args)
        {
            base.OnEnter(args);
            UIManager.Instance.Open("Example_SaveTestUI");
        }
    }
}