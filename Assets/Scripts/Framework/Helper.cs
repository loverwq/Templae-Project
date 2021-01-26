using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 通用的辅助方法
/// </summary>
public static class Helper
{
    /// <summary>
    /// 根据文件夹地址获取所有的图片（.jpg/.png）的名称（不带扩展名）和路径
    /// </summary>
    /// <param name="path">文件夹路径</param>
    /// <returns>返回一个字典（Dictionary）</returns>
    public static Dictionary<string, string> GetPhotoPaths(string path)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string[] strAray = System.IO.Directory.GetFiles(path);
        for (int i = 0; i < strAray.Length; i++)
        {
            FileInfo fileInfo = new FileInfo(strAray[i]);
            if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png")
            {
                string fileName = Path.GetFileNameWithoutExtension(strAray[i]);
                dic.Add(fileName, strAray[i]);
            }
        }
        return dic;
    }

    /// <summary>
    /// 根据文件夹地址获取文件夹下所有的图片（.jpg/.png）的地址
    /// </summary>
    /// <param name="path">文件夹地址</param>
    /// <returns></returns>
    public static List<string> GetPhotoPath(string path)
    {
        List<string> pathList = new List<string>();
        string[] strAray = System.IO.Directory.GetFiles(path);
        for (int i = 0; i < strAray.Length; i++)
        {
            FileInfo fileInfo = new FileInfo(strAray[i]);
            if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png")
            {
                pathList.Add(strAray[i]);
            }
        }
        return pathList;
    }

    /// <summary>
    /// 输出类对象的属性、字段、数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">类型对象</param>
    public static void PrintProperties<T>(T t)
    {
        var type = t.GetType();
        string temp = t.ToString() + "\n" + "{" + "\n";
        foreach (var p in type.GetProperties())
        {
            temp += "\t" + p.PropertyType + " " + p.Name + " = " + p.GetValue(t) + "\n";
        }
        Debug.Log(temp + "}");
    }

}
