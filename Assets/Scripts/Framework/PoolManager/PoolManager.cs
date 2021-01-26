using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ScriptSingleton<PoolManager>
{
    public GameObject[] prePrefabs;
    private Dictionary<string, ObjectPool> dic = new Dictionary<string, ObjectPool>();

    //private void Awake()
    //{
    //    if (_instance == null) _instance = this;
    //    else Destroy(gameObject);

    //    InitPool();
    //}

    //void InitPool()
    //{
    //    foreach (var prefab in prePrefabs)
    //    {
    //        CreatePool(prefab.name, prefab);
    //    }
    //}

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="name">对象池名字</param>
    /// <param name="prefab">对象池对应的预制体</param>
    public void CreatePool(string poolName, GameObject prefab)
    {
        if (!dic.ContainsKey(poolName))
        {
            dic[poolName] = new ObjectPool(prefab);
        }
    }

    /// <summary>
    /// 获取对象池
    /// </summary>
    /// <param name="poolName">对象池名字</param>
    /// <returns></returns>
    public GameObject Get(string poolName)
    {
        ObjectPool pool;
        if (dic.TryGetValue(poolName, out pool))
        {
            return pool.Get();
        }
        else
        {
            Debug.LogError($"对象池：{poolName}不存在！！！");
            return null;
        }
    }

    /// <summary>
    /// 回收对象池
    /// </summary>
    /// <param name="poolName">对象池名字</param>
    /// <param name="go">对象</param>
    public void Save(string poolName,GameObject go)
    {
        ObjectPool pool;
        if (dic.TryGetValue(poolName,out pool))
        {
            pool.Save(go);
        }
        else
        {
            Debug.LogError($"对象池：{poolName}不存在！！！");
        }
    }

}
