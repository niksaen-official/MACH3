using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneController : BaseUiController
{
    [SerializeField] GameObject dialogPrefab;
    [SerializeField] GameObject tutorialDialog;
    [SerializeField] GameFieldController gameFieldController;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI movesRemainingText;
    [SerializeField] LocalDataManager localDataManager;
    DialogController dialogController;
    
    bool endGame = false;
    bool isPaused = false;
    int timeInSeconds = 0;

    private void Start()
    {
        localDataManager.Load();
        tutorialDialog.GetComponent<TutorialDialogController>().callback = delegate
        {
            StartCoroutine("IncreaseTime");
        };
    }

    private void Update()
    {
        pointsText.text = $"Очки: {gameFieldController.points}";
        movesRemainingText.text = $"Осталось ходов: {gameFieldController.movesRemaining}";
        if(gameFieldController.movesRemaining == 0 && !endGame)
        {
            endGame = true;
            RecordData recordData = new(System.DateTime.Now,timeInSeconds,gameFieldController.points);
            if (localDataManager.Add(recordData))
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                dialogController = new();
                dialogController.Attach(Instantiate(dialogPrefab));
                dialogController.SetMessageText("Игра окончена");
                dialogController.SetCancelButtonText("Заново");
                dialogController.SetOkButtonText("Ok");
                dialogController.SetOkButtonOnClickAction(delegate { base.BackButtonOnClick(); });
                dialogController.SetCancelButtonOnClickAction(delegate { SceneManager.LoadScene(1); });
                dialogController.Show();
            }
        }
    }

    public override void BackButtonOnClick()
    {
        isPaused = true;
        dialogController = new();
        dialogController.Attach(Instantiate(dialogPrefab));
        dialogController.SetMessageText("Вы хотите выйти в меню?");
        dialogController.SetCancelButtonText("Остатсья");
        dialogController.SetOkButtonText("В меню");
        dialogController.SetOkButtonOnClickAction(delegate { base.BackButtonOnClick(); });
        dialogController.SetCancelButtonOnClickAction(delegate {
            isPaused = false;
            dialogController.Dissmis();
        });
        dialogController.Show();
    }

    IEnumerator IncreaseTime()
    {
        while(!isPaused)
        {
            yield return new WaitForSeconds(1);
            timeInSeconds++;
        }
    }
}
