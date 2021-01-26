using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计时器管理类
/// </summary>
public class TimerManager : ScriptSingleton<TimerManager>
{
    private List<TimerTask> timerTasks = new List<TimerTask>();
    private List<FrameTask> frameTasks = new List<FrameTask>();

    private List<int> tIDs = new List<int>();

    private int tID;

    private static readonly string obj = "lock";

    private int frame;

    private void Update()
    {
        frame++;
        TimeFunc();
        FrameFunc();
    }

    private void FrameFunc()
    {
        for (int i = 0; i < frameTasks.Count; i++)
        {
            if (frameTasks[i].bestTime < frame)
            {
                if (frameTasks[i].callback != null)
                {
                    frameTasks[i].callback();
                    frameTasks[i].bestTime = frame + frameTasks[i].delayTime;
                    if (frameTasks[i].currNum == -1)
                    {
                        continue;
                    }
                    else
                    {
                        frameTasks[i].currNum--;
                        if (frameTasks[i].currNum == 0)
                        {
                            frameTasks.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
    }

    private void TimeFunc()
    {
        for (int i = 0; i < timerTasks.Count; i++)
        {
            if (timerTasks[i].bestTime < Time.realtimeSinceStartup * 1000)
            {
                if (timerTasks[i].callback != null)
                {
                    timerTasks[i].callback();
                    timerTasks[i].bestTime = Time.realtimeSinceStartup * 1000 + timerTasks[i].delayTime;
                    if (timerTasks[i].currNum == -1)
                    {
                        continue;
                    }
                    else
                    {
                        timerTasks[i].currNum--;
                        if (timerTasks[i].currNum == 0)
                        {
                            timerTasks.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 添加一个时间定时任务
    /// </summary>
    /// <param name="delayTime">间隔时间</param>
    /// <param name="action">回调函数</param>
    /// <param name="timeType">时间类型</param>
    /// <param name="num">-1为无限循环,其余整数为循环次数</param>
    /// <returns>时间定时任务id</returns>
    public int AddTimerTask(float delayTime, Action action, TimeType timeType = TimeType.Millisecond, int num = 1)
    {
        if (timeType != TimeType.Millisecond)
        {
            switch (timeType)
            {
                case TimeType.Millisecond:
                    break;
                case TimeType.Second:
                    delayTime *= 1000;
                    break;
                case TimeType.Minute:
                    delayTime *= 60;
                    break;
                case TimeType.Hour:
                    delayTime *= 3600;
                    break;
                case TimeType.Day:
                    delayTime *= (3600 * 24);
                    break;
                default:
                    break;
            }
        }
        int id = GetTID();
        TimerTask timerTask = new TimerTask
        {
            bestTime = Time.realtimeSinceStartup * 1000 + delayTime,
            callback = action,
            currNum = num,
            delayTime = delayTime
        };
        timerTask.tID = id;
        timerTasks.Add(timerTask);
        tIDs.Add(id);
        return timerTask.tID;
    }

    /// <summary>
    /// 删除一个时间定时任务
    /// </summary>
    /// <param name="tID">时间定时任务id</param>
    /// <returns>是否删除成功</returns>
    public bool DelTimerTask(int tID)
    {
        bool isSucced = false;
        for (int i = 0; i < timerTasks.Count; i++)
        {
            if (timerTasks[i].tID == tID)
            {
                timerTasks.RemoveAt(i);
                isSucced = true;
                tIDs.Remove(tID);
                break;
            }
        }
        return isSucced;
    }

    /// <summary>
    /// 重写已有的时间定时任务
    /// </summary>
    /// <param name="tID">时间定时任务id</param>
    /// <param name="delayTime">间隔时间</param>
    /// <param name="action">回调函数</param>
    /// <param name="timeType">时间类型</param>
    /// <param name="num">执行次数</param>
    /// <returns>是否重写成功</returns>
    public bool RepTimerTask(int tID, float delayTime, Action action, TimeType timeType = TimeType.Millisecond, int num = 1)
    {
        bool isSucced = false;
        for (int i = 0; i < timerTasks.Count; i++)
        {
            if (timerTasks[i].tID==tID)
            {
                if (timeType!=TimeType.Millisecond)
                {
                    switch (timeType)
                    {
                        case TimeType.Millisecond:
                            break;
                        case TimeType.Second:
                            delayTime *= 1000;
                            break;
                        case TimeType.Minute:
                            delayTime *= 60;
                            break;
                        case TimeType.Hour:
                            delayTime *= 3600;
                            break;
                        case TimeType.Day:
                            delayTime *= (3600 * 24);
                            break;
                        default:
                            break;
                    }
                }
                TimerTask timerTask = new TimerTask
                {
                    bestTime = Time.realtimeSinceStartup * 1000 + delayTime,
                    callback = action,
                    currNum = num,
                    delayTime = delayTime
                };
                timerTask.tID = GetTID();
                timerTasks[i] = timerTask;
                isSucced = true;
            }
        }
        return isSucced;
    }


    /// <summary>
    /// 添加一个帧定时任务
    /// </summary>
    /// <param name="delayTime">间隔多少帧</param>
    /// <param name="action">回调函数</param>
    /// <param name="num">执行次数</param>
    /// <returns>返回任务id</returns>
    public int AddFrameTask(int delayTime,Action action, int num=1)
    {
        int id = GetTID();
        FrameTask frameTask = new FrameTask
        {
            bestTime = frame + delayTime,
            callback = action,
            currNum = num,
            delayTime = delayTime
        };
        frameTask.tID = id;
        frameTasks.Add(frameTask);
        tIDs.Add(id);
        return frameTask.tID;
    }

    /// <summary>
    /// 删除一个定时帧任务
    /// </summary>
    /// <param name="tID">任务id</param>
    /// <returns>删除是否成功</returns>
    public bool DelFrameTimerTask(int tID)
    {
        bool isSucced = false;
        for (int i = 0; i < frameTasks.Count; i++)
        {
            if (frameTasks[i].tID == tID)
            {
                frameTasks.RemoveAt(i);
                isSucced = true;
                tIDs.Remove(tID);
                break;
            }
        }
        return isSucced;
    }

    /// <summary>
    /// 重写一个已有的帧定时任务
    /// </summary>
    /// <param name="tID">任务id</param>
    /// <param name="delayTime">间隔多少帧</param>
    /// <param name="action">回调函数</param>
    /// <param name="num">执行次数</param>
    /// <returns>是否重写成功</returns>
    public bool RepFrameTimerTask(int tID, int delayTime, Action action, int num = 1)
    {
        bool isSucced = false;
        for (int i = 0; i < frameTasks.Count; i++)
        {
            FrameTask timerTask = new FrameTask { bestTime = frame + delayTime, callback = action, currNum = num, delayTime = delayTime };
            timerTask.tID = GetTID();
            frameTasks[i] = timerTask;
            isSucced = true;
        }
        return isSucced;
    }

    /// <summary>
    /// 生成通用的任务id
    /// </summary>
    /// <returns></returns>
    private int GetTID()
    {
        lock (obj)
        {
            tID++;

            while (true)
            {
                if (tID==int.MaxValue)
                {
                    tID = 0;
                }
                bool used = false;
                for (int i = 0; i < tIDs.Count; i++)
                {
                    if (tIDs[i]==tID)
                    {
                        used = true;
                        break;
                    }
                }
                if (!used)
                {
                    break;
                }
                tID++;
            }
        }
        return tID;
    }

}
