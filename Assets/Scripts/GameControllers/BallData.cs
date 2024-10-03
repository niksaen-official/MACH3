using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BallData : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Image hint;
    [SerializeField] GameObject cursor;

    public Color color { get; private set; } = Color.white;
    public int row = -1;
    public int column = -1;
    public bool haveHint { get => hint.enabled; }

    public void UpdateColor(Color color)
    {
        this.color = color;
        img.color = color;
    }

    public void ChangeHint() => hint.enabled = !hint.enabled;
    public void ChangeCursor() => cursor.SetActive(!cursor.activeSelf);
}
