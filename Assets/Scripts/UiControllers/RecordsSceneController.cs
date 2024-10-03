using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordsSceneController : BaseUiController
{
    [SerializeField] GameObject list;
    [SerializeField] LocalDataManager localDataManager;
    public List<RecordData> records;
    

    private void Start()
    {
        localDataManager.Load();
        list.GetComponent<RecordsListAdapter>().records = localDataManager.records;
        list.GetComponent<RecordsListAdapter>().Draw();
        localDataManager.NotifyData(list.GetComponent<RecordsListAdapter>().records);
    }
}
