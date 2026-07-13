using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI info;
    [SerializeField] TextMeshProUGUI playerScoreOne;
    [SerializeField] TextMeshProUGUI playerScoreTwo;
    [SerializeField] TextMeshProUGUI countDowntimer;

    [SerializeField] GameObject theBall;
    [SerializeField] GameObject gameManager;

    float timeElapsed = 0.0f;

    int playerOneScore = 0;
    public int scoreOne { get { return playerOneScore; } }
    int playerTwoScore = 0;
    public int scoreTwo { get { return playerTwoScore; } }
    int round = 1;
    bool winnerPlayer = false;
    public bool WinnerPlayer { get { return winnerPlayer; } }

    bool isGameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        theBall.SetActive(false);
        playerScoreOne.text = playerOneScore.ToString();
        playerScoreTwo.text = playerTwoScore.ToString();

        countDowntimer.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed = gameManager.GetComponent<GameTimer>().timeElapsed;
        if (!isGameStarted)
        {
            CountDownn((int)timeElapsed);
        }
        
        if (playerOneScore == 5 || playerTwoScore == 5)
        {
            if(playerOneScore -  playerTwoScore > 0)
            {
                GameOver(1);
            }
            else
            {
                GameOver(2);
            }
        }
        else
        {
            isPoint();
        }
    }

    void CountDownn(int timer)
    {
        if (timer < 2)
        {
            countDowntimer.text = "Game Begins!";
        }
        else if (timer < 3)
        {
            countDowntimer.text = "Round\n" + round.ToString();
        }
        else if (timer < 4)
        {
            countDowntimer.text = "READY!!";
        }
        else
        {
            countDowntimer.text = (7 - (int)timer).ToString();
            if (7 - (int)timer == 0)
            {
                countDowntimer.gameObject.SetActive(false);
                theBall.SetActive(true);
                isGameStarted = true;
            }
        }

    }

    void isPoint()
    {
        if (theBall.transform.position.x > 8.5f)
        {
            theBall.SetActive(false);
            round++;
            playerOneScore++;
            playerScoreOne.text = playerOneScore.ToString();
            winnerPlayer = false;
            StartCoroutine(NextRound());

        }
        if (theBall.transform.position.x < -8.5f)
        {
            theBall.SetActive(false);
            round++;
            playerTwoScore++;
            playerScoreTwo.text = playerTwoScore.ToString();
            winnerPlayer = true;
            StartCoroutine(NextRound());

        }

    }

    IEnumerator NextRound()
    {
        theBall.transform.position = Vector3.zero;        
        countDowntimer.gameObject.SetActive(true);
        countDowntimer.text = "SCORE!!";
        yield return new WaitForSeconds(2.0f);        
        TimerReset();
        isGameStarted = false;

    }

    void GameOver(int player)
    {
        theBall.SetActive(false);
        countDowntimer.text = "Player " + player.ToString() + "\nWins!";
    }

    void TimerReset()
    {
        gameManager.GetComponent<GameTimer>().timeElapsed = 0.0f;
    }
}
