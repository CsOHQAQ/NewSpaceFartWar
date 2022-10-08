using QxFramework.Core;

public class Titlemodule : Submodule {

    protected override void OnInit()
    {
        base.OnInit();
        InitGame();
    }
    private void InitGame()
    {
        UIManager.Instance.Open("HintUI");
        QXData.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
    }
}
