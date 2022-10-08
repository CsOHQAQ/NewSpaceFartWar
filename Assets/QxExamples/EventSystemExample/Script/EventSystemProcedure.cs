using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        QXData.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
        UIManager.Instance.Open("HintUI");
        GameMgr.Get<IEventManager>().ForceEvent(100);
    }
}
