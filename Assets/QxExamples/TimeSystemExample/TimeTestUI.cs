using App.Common;
using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTestUI : UIBase
{
    public int RepeatTime = 60;

    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        CollectObject();
        Rgst();
    }
    void Rgst()
    {
        GameMgr.Get<IGameTimeManager>().RegisterTimeRepeat(RegistAction, GameDateTime.ByMinutes(RepeatTime));

        _gos["StepBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["StepBtn"].GetComponent<Button>().onClick.AddListener(() => {
            GameMgr.Get<IGameTimeManager>().StepMinute(60);
        });

        _gos["SpeedUpBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["SpeedUpBtn"].GetComponent<Button>().onClick.AddListener(() => {
            App.Common.Data.Instance.TimeSize *= 0.75f; 
        });
        _gos["SpeedDownBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["SpeedDownBtn"].GetComponent<Button>().onClick.AddListener(() => {
            App.Common.Data.Instance.TimeSize *= 1.25f;
        });
        _gos["PauseBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["PauseBtn"].GetComponent<Button>().onClick.AddListener(() => {
            if (GameMgr.Get<IGameTimeManager>().IsPlaying())
            { GameMgr.Get<IGameTimeManager>().Pause(); }
            else
            {
                App.Common.Data.Instance.TimeSize = 1;
                GameMgr.Get<IGameTimeManager>().DoStart();
            }
        });
    }
    private void Update()
    {
        _gos["TimeText"].GetComponent<Text>().text = GameMgr.Get<IGameTimeManager>().GetNow().ToDurationString();
    }
    bool RegistAction(GameDateTime gameDateTime)
    {
        Debug.Log(string.Format("注册刷新事件，刷新时间：{0}", RepeatTime));
        return true;
    }
}
