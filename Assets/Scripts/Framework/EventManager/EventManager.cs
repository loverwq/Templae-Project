using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : ScriptSingleton<EventManager>
{
    private Dictionary<EventType, Delegate> m_dic = new Dictionary<EventType, Delegate>();

    public void AddListen(EventType eventType, Callback callback)
    {
        if (!m_dic.ContainsKey(eventType))
        {
            m_dic.Add(eventType, null);
        }
        Delegate d = m_dic[eventType];
        if (d != null && d.GetType() != callback.GetType())
        {
            throw new Exception("类型不同");
        }
        m_dic[eventType] = (Callback)m_dic[eventType] + callback;
    }

    public void RemoveListen(EventType eventType, Callback callback)
    {
        if (m_dic.ContainsKey(eventType))
        {
            Delegate d = m_dic[eventType];
            if (d==null)
            {
                throw new Exception("没有对应的类型事件");
            }
            else if (d.GetType()!=callback.GetType())
            {
                throw new Exception("类型不同");
            }
        }
        else
        {
            throw new Exception("没有对应的类型事件");
        }
        m_dic[eventType] = (Callback)m_dic[eventType] - callback;
    }

    public void Broadcast(EventType eventType)
    {
        Delegate d;
        if (m_dic.TryGetValue(eventType,out d))
        {
            Callback callback = (Callback)d;
            if (callback !=null)
            {
                callback();
            }
            else
            {
                throw new Exception("Callback为空！！！");
            }
        }
    }
}
