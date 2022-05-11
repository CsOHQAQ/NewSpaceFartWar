using App.Common;
using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBreifUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        CollectObject();
        ShowWord(args as string[]);
    }
    private void ShowWord(string[] Words)
    {
        _gos["TitleText"].GetComponent<Text>().text = Words[0];
        _gos["ContentText"].GetComponent<Text>().text = Words[1];
    }
}
