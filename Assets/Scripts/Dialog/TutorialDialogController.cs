using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialDialogController : MonoBehaviour
{
    [SerializeField] GameObject gameField;
    [SerializeField] GameObject rulesText;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI buttonText;
    GameFieldController gameFieldController;
    int pos = 0;
    BallData clickedBall;

    public Action callback;

    void Start()
    {
        gameFieldController = gameField.GetComponent<GameFieldController>();
        clickedBall = gameFieldController.AddMatches();
        gameObject.SetActive(true);
        PlayGameGuide();
    }
    public void NextButtonOnClick()
    {
        titleText.text = "Правила";
        gameField.SetActive(false);
        rulesText.SetActive(true);
        buttonText.text = "Закрыть";
        pos++;
        if(pos == 2)
        {
            gameObject.SetActive(false);
            callback();
        }
    }

    void PlayGameGuide()
    {
        StartCoroutine(GameGuide());
    }

    IEnumerator GameGuide()
    {
        yield return new WaitForSeconds(1);
        clickedBall.ChangeCursor();
        yield return new WaitForSeconds(1);
        clickedBall.ChangeCursor();
        yield return new WaitForSeconds(1);
        gameFieldController.OnClickedBall(clickedBall);
    }
}
