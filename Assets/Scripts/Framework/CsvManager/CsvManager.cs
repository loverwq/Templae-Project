using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>
/// CSV文件读取
/// </summary>
public class CsvManager : ScriptSingleton<CsvManager>
{
    /// <summary>
    /// 存放所有CSV数据缓存
    /// </summary>
    private Dictionary<Type, object> csvDatas = new Dictionary<Type, object>();

    /// <summary>
    /// 读取文件，结果保存到一个string数组，数组的每一行就是文件的每一行
    /// （只支持PC和Android，不支持iOS，因为木头不玩iOS）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    public string[] LoadFileLines(string filePath)
    {
        string url = Application.streamingAssetsPath + "/" + filePath;
#if UNITY_EDITOR
        return File.ReadAllLines(url);
#elif UNITY_ANDROID
        WWW www = new WWW(url);
        while (!www.isDone) { }
        return www.text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
#else
        return File.ReadAllLines(url);
#endif
    }

    /// <summary>
    /// 缓存CSV文件
    /// </summary>
    /// <typeparam name="T_CsvData">CSV数据对象类型</typeparam>
    /// <param name="filePath">CSV文件路径，确保CSV文件存放在Assets/StreamingAssets目录（或子目录）</param>
    public void CachedCsvFile<T_CsvData>(string filePath)
    {
        csvDatas[typeof(T_CsvData)] = LoadCsvData<T_CsvData>(filePath);
    }

    /// <summary>
    /// 获取某个CSV文件的所有数据，返回字典集合，Key为ID，Value为每一行的数据对象
    /// </summary>
    /// <typeparam name="T_CsvData">文件数据类型</typeparam>
    /// <returns></returns>
    public Dictionary<int, T_CsvData> GetCsvFileDatas<T_CsvData>()
    {
        Type type = typeof(T_CsvData);

        if (csvDatas.ContainsKey(type) == true)
        {
            /* 取出存放该数据对象类型的字典 */
            object dicObj = csvDatas[type];

            return (Dictionary<int, T_CsvData>)dicObj;
        }

        return new Dictionary<int, T_CsvData>();
    }

    /// <summary>
    /// 读取CSV文件数据（利用反射）
    /// </summary>
    /// <typeparam name="CsvData">CSV数据对象的类型</typeparam>
    /// <param name="csvFilePath">CSV文件路径</param>
    /// <param name="csvDatas">用于缓存数据的字典</param>
    /// <returns>CSV文件所有行内容的数据对象</returns>
    private Dictionary<int, T_CsvData> LoadCsvData<T_CsvData>(string csvFilePath)
    {
        Dictionary<int, T_CsvData> dic = new Dictionary<int, T_CsvData>();

        /* 从CSV文件读取数据 */
        Dictionary<string, Dictionary<string, string>> result = LoadCsvFile(csvFilePath);

        /* 遍历每一行数据 */
        foreach (string ID in result.Keys)
        {
            /* CSV的一行数据 */
            Dictionary<string, string> datas = result[ID];

            /* 读取Csv数据对象的属性 */
            PropertyInfo[] props = typeof(T_CsvData).GetProperties();

            /* 使用反射，将CSV文件的数据赋值给CSV数据对象的相应字段，要求CSV文件的字段名和CSV数据对象的字段名完全相同 */
            T_CsvData obj = Activator.CreateInstance<T_CsvData>();
            foreach (PropertyInfo p in props)
            {
                PiSetValue<T_CsvData>(datas[p.Name], p, obj);
            }

            /* 按ID-数据的形式存储 */
            dic[Convert.ToInt32(ID)] = obj;
        }

        return dic;
    }

    /// <summary>
    /// 读取CSV文件
    /// 结果保存到字典集合，以ID作为Key值，对应每一行的数据，每一行的数据也用字典集合保存。
    /// </summary>
    /// <param name="filePath">CSV文件路径</param>
    /// <returns></returns>
    public Dictionary<string, Dictionary<string, string>> LoadCsvFile(string filePath)
    {
        Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();

        string[] fileData = LoadFileLines(filePath);

        if (fileData.Length < 3)
        {
            return result;
        }

        /* CSV文件的第一行为Key字段，第二行为说明（不需要读取），第三行开始是数据。第一个字段一定是ID。 */
        string[] keys = fileData[0].Split(',');
        for (int i = 2; i < fileData.Length; i++)
        {
            string[] line = fileData[i].Split(',');

            /* 以ID为key值，创建一个新的集合，用于保存当前行的数据 */
            string ID = line[0];
            result[ID] = new Dictionary<string, string>();
            for (int j = 0; j < line.Length; j++)
            {
                /* 每一行的数据存储规则：Key字段-Value值 */
                result[ID][keys[j]] = line[j];
            }
        }

        return result;
    }

    /// <summary>
    /// 给属性赋值
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="value">值</param>
    /// <param name="pi">反射的属性对象</param>
    /// <param name="obj">被赋值的对象</param>
    public void PiSetValue<T>(string value, PropertyInfo pi, T obj)
    {
        /* ChangeType无法强制转换可空类型，所以需要这样特殊处理（参考：http://bbs.csdn.net/topics/320213735） */
        if (pi.PropertyType.FullName.IndexOf("Boolean") > 0)
        {
            /* 布尔类型要特殊处理  */
            pi.SetValue(obj, Convert.ChangeType(Convert.ToInt16(value), (Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType)), null);
        }
        else
        {
            pi.SetValue(obj, Convert.ChangeType(value, (Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType)), null);
        }
    }

}
