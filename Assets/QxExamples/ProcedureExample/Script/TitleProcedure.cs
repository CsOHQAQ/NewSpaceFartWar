using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleProcedure : ProcedureBase {

    protected override void OnEnter(object args)
    {
        AddSubmodule(new Titlemodule());
        base.OnEnter(args);
        ChangeTo(Launcher.Instance.SecondProcedure);
    }
}
