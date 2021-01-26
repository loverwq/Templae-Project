using System;

/// <summary>
/// 关于时间定时的任务
/// </summary>
public class TimerTask
{
    public Action callback;
    public float bestTime;
    public int currNum;
    public float delayTime;
    public int tID;
}

/// <summary>
/// 关于帧定时的任务
/// </summary>
public class FrameTask
{
    public Action callback;
    public int bestTime;
    public int currNum;
    public int delayTime;
    public int tID;
}

/// <summary>
/// 时间定时的时间单位
/// </summary>
public enum TimeType
{
    Millisecond,
    Second,
    Minute,
    Hour,
    Day
}
