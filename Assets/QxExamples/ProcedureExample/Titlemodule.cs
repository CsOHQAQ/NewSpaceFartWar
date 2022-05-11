using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Common;

public class Titlemodule : Submodule {

    protected override void OnInit()
    {
        base.OnInit();
        InitGame();
    }
    private void InitGame()
    {
        UIManager.Instance.Open("HintUI");
        Data.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
    }
}
