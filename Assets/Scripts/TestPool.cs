using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour,IPoolRelease
{
    float t=0;
    public void OnReset()
    {
        t = 0;
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t>5)
        {
            PoolManager.Instance.Save("123", gameObject);
        }
    }
}
