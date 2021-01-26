using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    private int tID;

    void Start()
    {
        TimerManager.Instance.AddTimerTask(3, TestTimerTask, TimeType.Second, 3);
        tID = TimerManager.Instance.AddFrameTask(1, SayHello, -1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimerManager.Instance.RepFrameTimerTask(tID, 200, () => Debug.Log("RepFrameTimerTask!!!"));
        }
    }
    private void TestTimerTask()
    {
        Debug.Log("Hello,TimerTask!!!");
    }

    private void SayHello()
    {
        Debug.Log("Hello,FrameTask!!!");
    }
}
