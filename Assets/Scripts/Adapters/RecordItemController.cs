using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordItemController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] Image background;

    public void SetData(RecordData data)
    {
        dateText.text = data.ParseDate();
        pointsText.text = data.points.ToString();
        timeText.text = data.ParseTime();
        if(data.isNew)
        {
            background.color = Color.yellow;
            data.isNew = false;
        }
    }
}
