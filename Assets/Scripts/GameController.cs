using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    
    public static GameController Instance;

    public GameObject startMenu;
    public GameObject endGame;
    public GameObject gameCountdown;
    public Text scoreText;

    enum PageState
    {
        None,
        Start,
        EndGame,
        GameCountdown
    }

    int score = 0;
    bool gameIsOver = true;

    public bool GameOver { get { return gameIsOver; } }
    public int Score { get { return score;  } }

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        PlaneControl.OnPlayerCrashed += OnPlayerCrashed;
    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        PlaneControl.OnPlayerCrashed -= OnPlayerCrashed;
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameIsOver = false;
    }

    void OnPlayerCrashed()
    {
        gameIsOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");

        // Check for new high score
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.EndGame);
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startMenu.SetActive(false);
                endGame.SetActive(false);
                gameCountdown.SetActive(false);
                break;
            case PageState.Start:
                startMenu.SetActive(true);
                endGame.SetActive(false);
                gameCountdown.SetActive(false);
                break;
            case PageState.EndGame:
                startMenu.SetActive(false);
                endGame.SetActive(true);
                gameCountdown.SetActive(false);
                break;
            case PageState.GameCountdown:
                startMenu.SetActive(false);
                endGame.SetActive(false);
                gameCountdown.SetActive(true);
                break;
        }
    }

    void ConfirmEndGame()
    {
        //Activated when replay button is pressed
        OnGameOverConfirmed(); // Event
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    void StartGame()
    {
        //Activated when play button is pressed
        SetPageState(PageState.GameCountdown);
    }

    public void incScore()
    {
        this.score++;
    }
}
