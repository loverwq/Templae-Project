using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Unity扩展方法
/// </summary>
public static class CommonExtension
{
    /// <summary>
    /// 显示子节点，根据所给的数量显示子节点的个数
    /// </summary>
    /// <param name="transform">父节点</param>
    /// <param name="count">要显示的数量</param>
    public static void ShowChildOfCount(this Transform transform, int count)
    {
        if (count > transform.childCount)
        {
            Debug.LogError("超出索引范围！！！");
            return;
        }
        for (int i = 0; i < count; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy == false)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 显示对象
    /// </summary>
    /// <param name="transform">要显示的对象</param>
    public static void Show(this Transform transform)
    {
        if (transform.gameObject.activeInHierarchy == false)
        {
            transform.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 隐藏对象
    /// </summary>
    /// <param name="transform">要隐藏的对象</param>
    public static void Hide(this Transform transform)
    {
        if (transform.gameObject.activeInHierarchy == true)
        {
            transform.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 显示所有的子节点
    /// </summary>
    /// <param name="transform">父节点</param>
    public static void ShowAllChild(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).Show();
        }
    }

    /// <summary>
    /// 隐藏所有的子节点
    /// </summary>
    /// <param name="transform">父节点</param>
    public static void HideAllChild(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).Hide();
        }
    }

    /// <summary>
    /// 根据子节点的名称显示
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name">子节点的名称</param>
    public static void ShowChildOfName(this Transform transform, string name)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == name)
            {
                transform.GetChild(i).Show();
                return;
            }
        }
        Debug.LogError($"未找到子节点为{name}的对象！！！");
    }

    /// <summary>
    /// 根据子节点的名称隐藏子节点
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name">子节点名称</param>
    public static void HideChildOfName(this Transform transform, string name)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == name)
            {
                transform.GetChild(i).Hide();
                return;
            }
        }
        Debug.LogError($"未找到子节点为{name}的对象！！！");
    }

    /// <summary>
    /// 获取所有的子节点
    /// </summary>
    /// <param name="transform"></param>
    /// <returns>返回一个包含所有子节点的list</returns>
    public static List<GameObject> GetAllChild(this Transform transform)
    {
        List<GameObject> childList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i).gameObject);
        }
        return childList;
    }

}
