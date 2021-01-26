using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 配置实例类
/// </summary>
[System.Serializable]
public class ConfigInfo
{
    public int 间隔时间=5;
}


public class ConfigMgr : MonoBehaviour
{
    public static ConfigMgr Instance;
    [HideInInspector]
    public ConfigInfo configInfo = null;
    private string path;

    private void Awake()
    {
        Instance = this;

        path = Application.dataPath + "/StreamingAssets/configInfo.xml";
        LoadConfigInfo(path);//加载配置文件
    }

    void Start()
    {

    }

    private void LoadConfigInfo(string path)
    {
        if (File.Exists(path))
        {
            FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string text = sr.ReadToEnd();
            configInfo = XmlSerializeHelper.DeSerialize<ConfigInfo>(text);
            fs.Close();
        }
    }

    /// <summary>
    /// 保存配置信息到本地
    /// </summary>
    [ContextMenu("保存配置文件")]
    public void SaveConfigInfo()
    {
        configInfo = new ConfigInfo();//构造当前的信息
        string xml = XmlSerializeHelper.Serialize<ConfigInfo>(configInfo);

        path = Application.dataPath + "/StreamingAssets/configInfo.xml";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
        }

        StreamWriter sw = new StreamWriter(path);
        sw.Write(xml);
        sw.Flush();
        sw.Close();

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

    }

    /// <summary>
    /// 保存配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="path"></param>
    public void SaveConfigInfo<T>(T obj,string path)
    {
        string xml = XmlSerializeHelper.Serialize<T>(obj);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
        }
        StreamWriter sw = new StreamWriter(path);
        sw.Write(xml);
        sw.Flush();
        sw.Close();
    }

    public T LoadConfigInfo<T>(string path) where T : new()
    {
        T t = default;
        if (File.Exists(path))
        {
            FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string text = sr.ReadToEnd();
            t = XmlSerializeHelper.DeSerialize<T>(text);
            fs.Close();
        }
        return t;
    }
}