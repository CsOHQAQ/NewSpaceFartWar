using QxFramework.Core;
using UnityEngine;
using UnityEngine.UI;

public class TimeTestUI : UIBase
{
    public int RepeatTime = 60;

    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        Rgst();
    }
    void Rgst()
    {
        GameMgr.Get<IGameTimeManager>().RegisterTimeRepeat(RegistAction, GameDateTime.ByMinutes(RepeatTime));

        Get<Button>("StepBtn").onClick.SetListener(() => {
            GameMgr.Get<IGameTimeManager>().StepMinute(60);
        });

        Get<Button>("SpeedUpBtn").onClick.SetListener(() => {
            GameMgr.Get<IGameTimeManager>().TimeSize *= 0.75f;
        });

        Get<Button>("SpeedDownBtn").onClick.SetListener(() => {
            GameMgr.Get<IGameTimeManager>().TimeSize *= 1.25f;
        });

        Get<Button>("PauseBtn").onClick.SetListener(() => {
            if (GameMgr.Get<IGameTimeManager>().IsPlaying())
            {
                GameMgr.Get<IGameTimeManager>().Pause();
            }
            else
            {
                GameMgr.Get<IGameTimeManager>().TimeSize = 1;
                GameMgr.Get<IGameTimeManager>().DoStart();
            }
        });
    }
    private void Update()
    {
        Get<Text>("TimeText").text = GameMgr.Get<IGameTimeManager>().GetNow().ToDurationString();
    }
    bool RegistAction(GameDateTime gameDateTime)
    {
        Debug.Log(string.Format("注册刷新事件，刷新时间：{0}", RepeatTime));
        return true;
    }
}
