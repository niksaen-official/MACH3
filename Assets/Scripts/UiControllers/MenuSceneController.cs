using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneController : MonoBehaviour
{
    [SerializeField] GameObject dialogPrefab;
    DialogController dialogController;

    public void NewGameBtnOnClick() => SceneManager.LoadScene(1);

    public void RecordsTableBtnOnClick() => SceneManager.LoadScene(2);

    public void AboutBtnOnClick() => SceneManager.LoadScene(3);

    public void ExitBtnOnClick() 
    {
        if (dialogController == null)
        {
            dialogController = new();
            dialogController.Attach(Instantiate(dialogPrefab));
            dialogController.SetMessageText("Вы хотите выйти из игры?");
            dialogController.SetCancelButtonText("Остаться");
            dialogController.SetOkButtonText("Выйти");
            dialogController.SetOkButtonOnClickAction(delegate { Application.Quit(); });
            dialogController.SetCancelButtonOnClickAction(delegate { dialogController.Dissmis(); });
            dialogController.Show();
        }
        else dialogController.Show();
    }
}
