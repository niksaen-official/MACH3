using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameFieldController : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject gameField;
    [SerializeField] bool isGuide = false;
    public int columnCount = 10;
    public int rowCount = 18;

    public int points { get; private set; } = 0;
    public int movesRemaining { get; private set; } = 10;

    List<List<GameObject>> balls = new List<List<GameObject>>();
    int matches = 0;
    BallData HintedBall;

    Color[] colors =
    {
        Color.white,
        Color.black,
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        Color.magenta,
        Color.gray
    };

    private void Start() => Create();
    public void Create()
    {
        for (int row = 0; row < rowCount; row++)
        {
            balls.Add(new List<GameObject>());
            for (int column = 0; column < columnCount; column++)
            {
                Color color = colors[Random.Range(0, colors.Length - 1)];
                GameObject ball = Instantiate(ballPrefab, gameField.transform);
                BallData ballData = ball.GetComponent<BallData>();
                ballData.UpdateColor(color);
                ballData.row = row;
                ballData.column = column;
                if (!isGuide)
                {
                    ball.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickedBall(ballData); });
                }
                balls[row].Add(ball);
            }
        }
        if (!isGuide)
        {
            HintedBall = FindMove();
            if (HintedBall == null) HintedBall = AddMatches();
            HintedBall.ChangeHint();
        }
    }

    public BallData AddMatches()
    {
        int column = Random.Range(1, columnCount - 2);
        int row = Random.Range(1, rowCount - 2);
        int orientation = Random.Range(0, 1);
        if(orientation == 0) 
        {
            Color color = balls[row][column].GetComponent<BallData>().color;
            balls[row-1][column].GetComponent<BallData>().UpdateColor(color);
            balls[row+1][column].GetComponent<BallData>().UpdateColor(color);
        }
        else
        {
            Color color = balls[row][column].GetComponent<BallData>().color;
            balls[row][column - 1].GetComponent<BallData>().UpdateColor(color);
            balls[row][column + 1].GetComponent<BallData>().UpdateColor(color);
        }
        return balls[row][column].GetComponent<BallData>();
    }

    public void OnClickedBall(BallData ball)
    {
        if (HintedBall != null)
        {
            HintedBall.ChangeHint();
            HintedBall = null;
        }
        movesRemaining--;
        if (!RemoveBallsInRow(ball)) RemoveBallsInColumn(ball);
    }

    void MoveColumnAndAddBall(BallData ballData)
    {
        if (ballData.row > 0) 
        {
            for(int row = ballData.row; row > 0; row--)
            {
                ballData = balls[row][ballData.column].GetComponent<BallData>();
                Color buff = ballData.color;
                BallData prevBall = balls[row - 1][ballData.column].GetComponent<BallData>();
                ballData.UpdateColor(prevBall.color);
                prevBall.UpdateColor(buff);
            }
            BallData firstBall = balls[0][ballData.column].GetComponent<BallData>();
            firstBall.UpdateColor(Color.clear);
            firstBall.UpdateColor(colors[Random.Range(0,colors.Length-1)]);
        }
        else
        {
            ballData.UpdateColor(Color.clear);
            ballData.UpdateColor(colors[Random.Range(0, colors.Length - 1)]);
        }
    }

    bool RemoveBallsInRow(BallData ballData)
    {
        bool searchForward = true;
        bool searchBackward = true;
        int column = ballData.column+1;

        List<BallData> matches = new()
        {
            ballData
        };

        while (searchForward)
        {
            if (column == balls[ballData.row].Count) break;
            BallData nextBall = balls[ballData.row][column].GetComponent<BallData>();
            if (nextBall.color == ballData.color)
            {
                matches.Add(nextBall);
                column++;
                if (column == balls[ballData.row].Count) searchForward = false;
            }
            else break;
        }

        column = ballData.column - 1;
        while (searchBackward)
        {
            if (column < 0) break;
            BallData prevBall = balls[ballData.row][column].GetComponent<BallData>();
            if (prevBall.color == ballData.color)
            {
                matches.Add(prevBall);
                column--;
                if (column < 0) searchBackward = false;
            }
            else break;
        }

        if (matches.Count > 2)
        {
            this.matches = matches.Count;
            foreach (BallData ball in matches)
            {
                StartCoroutine(DestroyBallAnimation(balls[ball.row][ball.column], delegate
                {
                    MoveColumnAndAddBall(ball);
                }));
            }
            return true;
        }
        else return false;
    }
    void RemoveBallsInColumn(BallData ballData)
    {
        bool searchForward = true;
        bool searchBackward = true;
        int row = ballData.row + 1;

        List<BallData> matches = new()
        {
            ballData
        };

        while (searchForward)
        {
            if (row == balls.Count) break;
            BallData nextBall = balls[row][ballData.column].GetComponent<BallData>();
            if (nextBall.color == ballData.color)
            {
                matches.Add(nextBall);
                row++;
                if (row == balls.Count) searchForward = false;
            }
            else break;
        }
        

        row = ballData.row - 1;
        while (searchBackward)
        {
            if (row < 0) break;
            BallData prevBall = balls[row][ballData.column].GetComponent<BallData>();
            if (prevBall.color == ballData.color)
            {
                matches.Add(prevBall);
                row--;
                if (row < 0) searchBackward = false;
            }
            else break;
        }
        

        if (matches.Count > 2)
        {
            this.matches = matches.Count;
            BallData data = null;
            foreach (BallData ball in matches)
            {
                if(data == null) data = ball;
                else if(ball.row > data.row) data = ball;
            }
            for(int i = 0; i < matches.Count; i++)
            {
                BallData ball = matches[i];
                StartCoroutine(DestroyBallAnimation(balls[ball.row][ball.column], delegate
                {
                    MoveColumnAndAddBall(ball);
                }));
            }
        }
        else
        {
            StartCoroutine(DestroyBallAnimation(balls[ballData.row][ballData.column], delegate
            {
                MoveColumnAndAddBall(ballData);
            }));
        }
    }

    IEnumerator DestroyBallAnimation(GameObject ball, Action callback)
    {
        float delay = 0.02f;
        float scale = 1f;
        for (int i = 0; i < 10; i++)
        {
            scale -= 0.1f;
            yield return new WaitForSeconds(delay);
            ball.transform.localScale = new(scale, scale);
        }
        yield return new WaitForSeconds(0.1f);
        ball.transform.localScale = new(1f, 1f);
        callback();
        if (matches > 2)
        {
            movesRemaining += matches - 1;
            points += matches;
            matches = 0;
        }
    }

    BallData FindMove()
    {
        for (int row = 0; row < rowCount; row++)
        {
            BallData ball = FindMoveInRow(row);
            if (ball != null) return ball;
        }
        for (int column = 0; column < columnCount; column++)
        {
            BallData ball = FindMoveInColumn(column);
            if (ball != null) return ball;
        }
        return null;
    }

    bool HaveMove() => FindMove() != null;

    BallData FindMoveInColumn(int column)
    {
        List<BallData> matches = new List<BallData>();
        for (int row = 0; row < balls.Count; row++)
        {
            BallData ball = balls[row][column].GetComponent<BallData>();
            if (matches.Count == 0)
                matches.Add(ball);
            else
            {
                BallData prev = balls[row-1][column].GetComponent<BallData>();
                if (prev.color == ball.color)
                    matches.Add(prev);
                else
                    matches.Clear();
            }
        }
        if (matches.Count > 2)
            return matches[1];
        else
            return null;
    }
    BallData FindMoveInRow(int row)
    {
        List<BallData> matches = new List<BallData>();
        for(int column = 0; column < balls[row].Count; column++)
        {
            BallData ball = balls[row][column].GetComponent<BallData>();
            if (matches.Count == 0)
                matches.Add(ball);
            else
            {
                BallData prev = balls[row][column - 1].GetComponent<BallData>();
                if(prev.color == ball.color)
                    matches.Add(prev);
                else
                    matches.Clear();
            }
        }
        if(matches.Count > 2)
            return matches[1];
        else
            return null;
    }
}
