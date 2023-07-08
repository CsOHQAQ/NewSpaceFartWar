using QxFramework.Core;

public class Titlemodule : Submodule {

    protected override void OnInit()
    {
        base.OnInit();
        InitGame();
    }
    private void InitGame()
    {
        QXData.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
        LevelManager.Instance.OpenLevel("Game", LoadCompleted);
    }
    private void LoadCompleted(string levelName)
    {
        UIManager.Instance.Open("HealthBarUI");
    }
}
