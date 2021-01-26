using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public GameObject prefab;
    public enum TestEnum
    {
        Test1,
        Test2
    }
    // Start is called before the first frame update
    void Start()
    {

        //对象池的使用
        PoolManager.Instance.CreatePool("123", prefab);


        //读取csv的使用
        CsvManager.Instance.CachedCsvFile<EntityCsvData>("CSV\\Entity.csv");
        Dictionary<int, EntityCsvData> dataDic = CsvManager.Instance.GetCsvFileDatas<EntityCsvData>();
        foreach (EntityCsvData csvData in dataDic.Values)
        {
            Debug.Log(csvData.ID + ":" +csvData.Name);
        }
        print("=====================================");
        EntityCsvData test = new EntityCsvData();
        test.ID = 123;
        test.ModelID = 234;
        test.Name = "345";
        Helper.PrintProperties<EntityCsvData>(test);
    }

    private void Update()
    {
        //对象池的使用
        if (Input.GetMouseButtonDown(1))
        {
            PoolManager.Instance.Get("123");
        }
    }

}
