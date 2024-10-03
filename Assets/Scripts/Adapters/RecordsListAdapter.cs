using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordsListAdapter : MonoBehaviour
{
    [SerializeField] GameObject listItemPrefab;
    [SerializeField] GameObject parent;
    public List<RecordData> records;
    List<GameObject> items = new();

    public void NotifyDataChanged()
    {
        for (int i = 0; i < records.Count; i++)
        {
            GameObject item = items[i];
            RecordItemController controller = item.GetComponent<RecordItemController>();
            controller.SetData(records[i]);
        }
    }

    public void Draw()
    {
        for(int i  = 0; i < records.Count; i++)
        {
            GameObject item = Instantiate(listItemPrefab,parent.transform);
            RecordItemController controller = item.GetComponent<RecordItemController>();
            controller.SetData(records[i]);
            records[i].isNew = false;
            items.Add(item);
        }
    }

}
