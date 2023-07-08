using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryUI : UIBase
{
    private void Awake()
    {
        Get<Button>("Retry").onClick.AddListener(() => { ProcedureManager.Instance.ChangeTo<TitleProcedure>(); });
    }
}
