using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI info;
    [SerializeField] TextMeshProUGUI playerScoreOne;
    [SerializeField] TextMeshProUGUI playerScoreTwo;
    [SerializeField] TextMeshProUGUI countDowntimer;

    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;

    [SerializeField] GameObject theBall;
    [SerializeField] GameObject gameManager;

    float timeElapsed = 0.0f;
    float scoreLine = 8.5f;   

    int round = 1;
    [SerializeField] int gameScore = 5;
    int playerOneScore = 0;
    public int scoreOne { get { return playerOneScore; } }
    int playerTwoScore = 0;
    public int scoreTwo { get { return playerTwoScore; } }

    bool isGameStarted = false;
    bool isGameOver = false;
    bool winnerPlayer = false;
    public bool WinnerPlayer { get { return winnerPlayer; } }

    // Her turda oyunun başında yapılacak işlemler. Bu yüzden start değilde OnEnable kullanıldı.
    void OnEnable()
    {
        theBall.SetActive(false);
        playerScoreOne.text = playerOneScore.ToString();
        playerScoreTwo.text = playerTwoScore.ToString();

        countDowntimer.gameObject.SetActive(true);

        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    void Update()
    {

        timeElapsed = gameManager.GetComponent<GameTimer>().timeElapsed;
        if (!isGameOver)
        {
            GameStart();
        }
    }

    void GameStart()
    {
        if (!isGameStarted)
        {
            CountDownn((int)timeElapsed);
        }

        if (playerOneScore == gameScore || playerTwoScore == gameScore)
        {
            if (playerOneScore - playerTwoScore > 0)
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
        if (theBall.transform.position.x > scoreLine)
        {
            theBall.SetActive(false);
            round++;
            playerOneScore++;
            playerScoreOne.text = playerOneScore.ToString();
            winnerPlayer = false;
            StartCoroutine(NextRound());

        }
        if (theBall.transform.position.x < -scoreLine)
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
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        isGameOver = true;
    }

    void TimerReset()
    {
        gameManager.GetComponent<GameTimer>().timeElapsed = 0.0f;
    }

    public void DoExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void RestartGame()
    {
        //Aktif Sayfayı yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
