using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController
{
    GameObject dialog;
    Button okButton;
    Button cancelButton;
    TextMeshProUGUI messageText;
    TextMeshProUGUI okButtonText;
    TextMeshProUGUI cancelButtonText;

    public void Attach(GameObject dialog)
    {
        this.dialog = dialog;
        this.dialog.SetActive(false);

        Button[] buttons = dialog.GetComponentsInChildren<Button>();
        if (buttons[0].name == "OkButton")
        {
            okButton = buttons[0];
            cancelButton = buttons[1];
        }
        else
        {
            cancelButton = buttons[0];
            okButton = buttons[1];
        }

        messageText = dialog.GetComponentInChildren<TextMeshProUGUI>();
        okButtonText = okButton.GetComponentInChildren<TextMeshProUGUI>();
        cancelButtonText = cancelButton.GetComponentInChildren<TextMeshProUGUI>(); 
    }

    public void SetMessageText(string text) => messageText.text = text;
    public void SetOkButtonText(string text) => okButtonText.text = text;
    public void SetCancelButtonText(string text) => cancelButtonText.text = text;

    public void SetOkButtonOnClickAction(Action onClick)
    {
        okButton.onClick.AddListener(delegate { onClick(); });
    }
    public void SetCancelButtonOnClickAction(Action onClick)
    {
        cancelButton.onClick.AddListener(delegate { onClick(); });
    }

    public void Show() => dialog.SetActive(true);
    public void Dissmis() => dialog.SetActive(false);
}
