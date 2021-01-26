using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Stack<GameObject> pool = new Stack<GameObject>();

    public ObjectPool(GameObject _prefab)
    {
        prefab = _prefab;
    }


    /// <summary>
    /// 取对象
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        if (pool.Count>0)
        {
            GameObject go = pool.Pop();
            go.SetActive(true);
            return go;
        }
        else
        {
            GameObject go = GameObject.Instantiate(prefab);
            return go;
        }
    }

    /// <summary>
    /// 回收/存对象
    /// </summary>
    /// <param name="go">回收/存的对象</param>
    public void Save(GameObject go)
    {
        go.transform.SetParent(PoolManager.Instance.transform);
        go.SetActive(false);
        //调用回收对象的重置方法，（接口中定义）
        IPoolRelease ipool = go.GetComponent<IPoolRelease>();
        if (ipool!=null)
        {
            ipool.OnReset();
        }
        pool.Push(go);
    }
}
