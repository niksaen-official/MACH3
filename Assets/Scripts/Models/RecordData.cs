using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData
{
    public DateTime date { get; private set; }
    public int time { get; private set; }
    public int points { get; private set; }
    public bool isNew=false;

    public RecordData(DateTime date, int time, int points,bool isNew=false)
    {
        this.date = date;
        this.time = time;
        this.points = points;
        this.isNew = isNew;
    }

    public string ParseTime()
    {
        int hours = time / 3600;
        int minutes = (time - (time / 3600)) / 60;
        int seconds = time % 60;

        string secondsStr;
        if (seconds >= 10) secondsStr = $"{seconds}";
        else secondsStr = $"0{seconds}";

        string minutesStr;
        if (minutes >= 10) minutesStr = $"{minutes}";
        else minutesStr = $"0{minutes}";

        if (hours > 0) return $"{hours}:{minutesStr}:{secondsStr}";
        else return $"{minutesStr}:{secondsStr}";
    }

    public string ParseDate()
    {
        string days = date.Day.ToString();
        string month = date.Month.ToString();
        string year = date.Year.ToString();
        return $"{days}.{month}.{year}";
    }
}
