using App.Common;
using QxFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : UIBase
{
    [ChildValueBind("LoadBtn", nameof(Button.onClick))]
    Action OnLoadButton;

    [ChildValueBind("SaveBtn", nameof(Button.onClick))]
    Action OnSaveButton;

    [ChildValueBind("DisplayBtn", nameof(Button.onClick))]
    Action OnDisplayButton;
       
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        CollectObject();
        OnLoadButton = Load;
        OnSaveButton = Save;
        OnDisplayButton = Display;
        CommitValue();

    }
    public void Save()
    {
        UIManager.Instance.Open("DialogWindowUI",args: new DialogWindowUI.DialogWindowUIArg("提示", "是否保存数据", null, "确定", () => {
            GameMgr.Get<IMainDataManager>().RefreshNum(Get<Transform>("InputField").Find("Text").GetComponent<Text>().text);
            GameMgr.Get<IMainDataManager>().SaveTo();
        }));
    }
    public void Load()
    {
        UIManager.Instance.Open("DialogWindowUI", args: new DialogWindowUI.DialogWindowUIArg("提示", "是否载入存档", null, "确定", () => {
            GameMgr.Get<IMainDataManager>().LoadFrom();
        }));
    }
    public void Display()
    {
        Get<Transform>("BG").GetComponentInChildren<Text>().text = GameMgr.Get<IMainDataManager>().DisplayNum().ToString();
    }
}

