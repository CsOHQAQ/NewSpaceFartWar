using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : LogicModuleBase, IGameTimeManager
{
    private class TimeActionItem
    {
        public Func<GameDateTime, bool> Action;
        public int IntervalTime;
    }

    //用于累计和判断需要更新的时间
    private float _time = 0f;

    private readonly List<TimeActionItem> _repeatsActions = new List<TimeActionItem>();

    private bool _playing = false;

    public NowTimeData NowTime;
    private readonly List<Func<bool>> _pauseConditions = new List<Func<bool>>();

    /// <summary>
    /// 停止步进时间
    /// </summary>
    private bool _stopStep;

    public bool IsStop => !_playing;

    private float _timeSize = 1;
    public float TimeSize { get => _timeSize; set => _timeSize = value; }

    public override void Init()
    {
        base.Init();
        if (!RegisterData(out NowTime))
        {
            NowTime.Now.Days = 1;
            NowTime.Now.Hours = 7;
            NowTime.Now.Minutes = 40;
        }
        DoStart();
    }


    /// <summary>
    ///步进分钟数
    /// </summary>
    /// <param name="stepTime">步进分钟</param>
    /// <param name="filter">执行分组</param>
    /// <returns>返回是否成功，未被打断</returns>
    public bool StepMinute(float stepTime)
    {
        //复位标识
        _stopStep = false;

        _time += stepTime;
        while (_time >= 1f && !_stopStep)
        {
            //游戏时间增加
            NowTime.Now += 1;
            foreach (var item in _repeatsActions)
            {
                if (NowTime.Now.TotalMinutes % item.IntervalTime == 0)
                {
                    _stopStep = (!item.Action(NowTime.Now)) || _stopStep;
                }

                //是否被打断
                if (_stopStep)
                {
                    Debug.Log("[GameTimer] 打断于" + _time);
                    break;
                }
            }
            _time -= 1;
        }
        return !_stopStep;
    }

    /// <summary>
    /// 当前是白天
    /// </summary>
    /// <returns></returns>
    public bool IsDayTime()
    {
        return GetNow().Hours > 5 && GetNow().Hours < 18;
    }


    public void StopStep()
    {
        _stopStep = true;
        _playing = false;
    }

    public override void FixedUpdate()
    {
        _pauseConditions.RemoveAll((f) => !f());
        //判断一遍所有条件，去掉未能达成的
        if (_pauseConditions.Count > 0)
        {
            return;
        }

        //如果单纯的被停了
        if (_playing && TimeSize > 0)
        {
            StepMinute(Time.fixedDeltaTime / TimeSize);
        }
    }
    public override void Update()
    {
        base.Update();
        ControlTimeSpeed();
    }
    private void ControlTimeSpeed()
    {
    }

    /// <summary>
    /// 注册时间循环函数
    /// </summary>
    /// <param name="action"></param>
    public void RegisterTimeRepeat(Func<GameDateTime, bool> action, GameDateTime interval)
    {
        Debug.Log($"[{nameof(GameTimeManager)};注册刷新函数{interval.ToDurationString()}");
        _repeatsActions.Add(new TimeActionItem()
        {
            Action = action,
            IntervalTime = interval.TotalMinutes,
        });
    }

    /// <summary>
    /// 添加时间临时暂停条件，主要是为了防止所有弹窗都写一遍
    /// </summary>
    /// <param name="condition">条件函数</param>
    public void AddTempPauseCondition(Func<bool> condition)
    {
        _pauseConditions.Add(condition);
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        _playing = false;
    }

    /// <summary>
    /// 开始
    /// </summary>
    public void DoStart()
    {
        _playing = true;
    }

    public bool IsPlaying()
    {
        return _playing;
    }

    /// <summary>
    /// 获取当前时间
    /// </summary>
    /// <returns>返回当前时间</returns>
    public GameDateTime GetNow()
    {
        return NowTime.Now;
    }

}

[Serializable]
public class NowTimeData : GameDataBase
{
    public GameDateTime Now;
}

/// <summary>
/// 封装好的游戏时刻类
/// </summary>
[Serializable]
[XLua.LuaCallCSharp]
public struct GameDateTime
{
    [SerializeField]
    public int Days;

    [SerializeField]
    public int Hours;

    [SerializeField]
    public int Minutes;

    public const int MinutesPerHour = 60;

    public const int HoursPerDay = 24;

    /// <summary>
    /// 获取总时间
    /// </summary>
    public int TotalMinutes => (Days * HoursPerDay * MinutesPerHour) + Hours * MinutesPerHour + Minutes;

    public GameDateTime(int days, int hours, int minutes)
    {
        //这么从秒数重新计算是为了防止传入会溢出的值
        var totalMinutes = (days * HoursPerDay * MinutesPerHour) + hours * MinutesPerHour + minutes;
        Minutes = totalMinutes % MinutesPerHour;
        Hours = (totalMinutes - Minutes) / MinutesPerHour % HoursPerDay;
        Days = (totalMinutes - Minutes - Hours * MinutesPerHour) / MinutesPerHour / HoursPerDay;
    }

    public override string ToString()
    {
        return $"{Days}:{Hours}:{Minutes}";
    }

    /// <summary>
    /// 获取时长字符串，花费几天几小时几分钟
    /// </summary>
    /// <returns></returns>
    public string ToDurationString()
    {
        return (Days > 0 ? $"{Days}天" : "")
              + (Hours > 0 ? $"{Hours}时" : "")
                  + (Minutes > 0 ? $"{Minutes}分" : "");
    }

    /// <summary>
    /// 转化为时刻字符串  第几天几时几分
    /// </summary>
    /// <returns></returns>
    public string ToMomentString()
    {
        return $"{Days}天{Hours:00}:{Minutes:00}";
    }

    /// <summary>
    /// 使用分钟创建时间
    /// </summary>
    /// <param name="totalMinutes"></param>
    /// <returns></returns>
    public static GameDateTime ByMinutes(int totalMinutes)
    {
        var mins = totalMinutes % MinutesPerHour;
        var hours = (totalMinutes - mins) / MinutesPerHour % HoursPerDay;
        var days = (totalMinutes - mins - hours * MinutesPerHour) / MinutesPerHour / HoursPerDay;
        return new GameDateTime(days, hours, mins);
    }

    public static GameDateTime ByHours(int totalHours)
    {
        var hours = (totalHours) % HoursPerDay;
        var days = (totalHours - hours) / HoursPerDay;
        return new GameDateTime(days, hours, 0);
    }
    public bool IsDayTime()
    {
        return Hours > 5 && Hours < 18;
    }


    #region 运算符

    public static GameDateTime operator +(GameDateTime t1, GameDateTime t2)
    {
        return ByMinutes(t1.TotalMinutes + t2.TotalMinutes);
    }

    public static GameDateTime operator -(GameDateTime t1, GameDateTime t2)
    {
        return ByMinutes(t1.TotalMinutes - t2.TotalMinutes);
    }

    public static GameDateTime operator +(GameDateTime t1, int t2)
    {
        return ByMinutes(t1.TotalMinutes + t2);
    }

    public static GameDateTime operator -(GameDateTime t1, int t2)
    {
        return ByMinutes(t1.TotalMinutes - t2);
    }

    public static bool operator ==(GameDateTime t1, GameDateTime t2)
    {
        return t1.Minutes == t2.Minutes && t1.Hours == t2.Hours && t1.Days == t2.Days;
    }

    public static bool operator !=(GameDateTime t1, GameDateTime t2)
    {
        return !(t1 == t2);
    }

    public static bool operator >=(GameDateTime t1, GameDateTime t2)
    {
        return t1.TotalMinutes >= t2.TotalMinutes;
    }

    public static bool operator <=(GameDateTime t1, GameDateTime t2)
    {
        return t1.TotalMinutes <= t2.TotalMinutes;
    }

    public static bool operator >(GameDateTime t1, GameDateTime t2)
    {
        return t1.TotalMinutes > t2.TotalMinutes;
    }

    public static bool operator <(GameDateTime t1, GameDateTime t2)
    {
        return t1.TotalMinutes < t2.TotalMinutes;
    }

    public bool Equals(GameDateTime other)
    {
        return Days == other.Days && Hours == other.Hours && Minutes == other.Minutes;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is GameDateTime && Equals((GameDateTime)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Days;
            hashCode = (hashCode * 397) ^ Hours;
            hashCode = (hashCode * 397) ^ Minutes;
            return hashCode;
        }
    }

    #endregion 运算符

}
