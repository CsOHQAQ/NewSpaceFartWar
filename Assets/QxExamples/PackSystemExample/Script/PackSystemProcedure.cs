using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackSystemProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        QXData.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
        UIManager.Instance.Open("Example_PackTestUI");
    }
}
