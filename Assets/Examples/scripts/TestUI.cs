using App.Common;
using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        CollectObject();
        _gos["LoadBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["LoadBtn"].GetComponent<Button>().onClick.AddListener(Load);
        _gos["SaveBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["SaveBtn"].GetComponent<Button>().onClick.AddListener(Save);
        _gos["DisplayBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["DisplayBtn"].GetComponent<Button>().onClick.AddListener(Display);
    }
    public void Save()
    {
        UIManager.Instance.Open("DialogWindowUI", 0, args: new DialogWindowUI.DialogWindowUIArg("提示", "是否保存数据", null, "确定", () => {
            GameMgr.Get<IMainDataManager>().RefreshNum(_gos["InputField"].transform.Find("Text").GetComponent<Text>().text);
            GameMgr.Get<IMainDataManager>().SaveTo();
        }));


    }
    public void Load()
    {
        UIManager.Instance.Open("DialogWindowUI", 0, args: new DialogWindowUI.DialogWindowUIArg("提示", "是否载入存档", null, "确定", () => {
            GameMgr.Get<IMainDataManager>().LoadFrom();
        }));
    }
    public void Display()
    {
        _gos["BG"].GetComponentInChildren<Text>().text = GameMgr.Get<IMainDataManager>().DisplayNum().ToString();
    }
}

