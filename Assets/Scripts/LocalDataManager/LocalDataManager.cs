using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalDataManager : MonoBehaviour
{
    public List<RecordData> records { get; private set; } = new();
    public void Load()
    {
        if (PlayerPrefs.HasKey("records"))
            records = DeserializeList(PlayerPrefs.GetString("records"));
        else
        {
            records = GetDefault();
            PlayerPrefs.SetString("records", SerializeList(records));
            PlayerPrefs.Save();
        }
    }

    public void NotifyData(List<RecordData> records)
    {
        this.records = records;
        PlayerPrefs.SetString("records", SerializeList(records));
        PlayerPrefs.Save();
    }

    public bool Add(RecordData record)
    {
        for (int i = 0; i < records.Count; i++)
        {
            RecordData item = records[i];
            if (item.points < record.points || (item.points==record.points && item.time>record.time))
            {
                record.isNew = true;
                records.Insert(i, record);
                records.RemoveAt(records.Count - 1);
                PlayerPrefs.SetString("records", SerializeList(records));
                PlayerPrefs.Save();
                return true;
            }
        }
        return false;
    }

    private string SerializeList(List<RecordData> records)
    {
        string res = "";
        foreach (RecordData data in records)
        {
            res += data.date.ToString() + ";";
            res += data.time.ToString() + ";";
            res += data.points.ToString() + ";";
            res += data.isNew.ToString() + "\n";
        }
        return res;
    }
    private List<RecordData> DeserializeList(string data)
    {
        List<RecordData > records = new List<RecordData>();
        string[] entities = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach(string entity in entities)
        {
            string[] fields = entity.Split(';');
            RecordData record = new(DateTime.Parse(fields[0]), int.Parse(fields[1]), int.Parse(fields[2]), bool.Parse(fields[3]));
            records.Add(record);
        }
        return records;
    }

    private List<RecordData> GetDefault()
    {
        string data = Resources.Load<TextAsset>("DefaultRecordData").text;
        List<RecordData> records = DeserializeList(data);
        return records;
    }
}
