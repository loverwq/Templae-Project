using System;
using UnityEngine;

/// <summary>
/// Json辅助类
/// </summary>
public static class JsonHelper 
{
    /// <summary>
    /// 反序列化json字符为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T FromJson<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// 反序列化json字符为对象
    /// </summary>
    /// <param name="json"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object FromJson(string json, Type type)
    {
        return JsonUtility.FromJson(json,type);
    }

    /// <summary>
    /// 覆写
    /// </summary>
    /// <param name="json"></param>
    /// <param name="objectToOverwrite"></param>
    public static void FromJsonOverwrite(string json, object objectToOverwrite)
    {
         JsonUtility.FromJsonOverwrite(json,objectToOverwrite);
    }

    /// <summary>
    /// 实体对象序列化成json字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    /// <summary>
    /// 实体对象序列化成json字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="prettyPrint">如果为true,格式化输出以使其可读。如果为false，将输出格式化为最小size,默认值为false</param>
    /// <returns></returns>
    public static string ToJson(object obj, bool prettyPrint)
    {
        return JsonUtility.ToJson(obj,prettyPrint);
    }

}
