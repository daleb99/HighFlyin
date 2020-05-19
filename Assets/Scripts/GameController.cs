using System;
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

    /*
     * GameObjects to simulate different
     * game stages
     */
    public GameObject mainMenu;
    public GameObject startMenu;
    public GameObject endGame;
    public GameObject gameCountdown;
    public Text scoreText;

    /*
     * GameObject to display the elements of the game:
     *  - Background
     *  - Ground
     *  - Clouds
     *  - Airplane
     *  - Birds
     */
    public GameObject environment;

    enum PageState
    {
        None,
        MainMenu,
        Start,
        EndGame,
        GameCountdown
    }

    public int score = 0;
    public bool gameIsOver = true;

    public bool GameOver { get { return gameIsOver; } }
    public int Score { get { return score;  } }

    void Awake()
    {
        // When we start the game, hide the games environment
        // so we can display the main menu to the user.
        //environment.SetActive(false);
        Instance = this;
    }

    // Subscribe to the events
    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        PlaneControl.OnPlayerCrashed += OnPlayerCrashed;
    }

    // Unsubscribe from events
    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        PlaneControl.OnPlayerCrashed -= OnPlayerCrashed;
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        scoreText.gameObject.SetActive(true);
        score = 0;
        gameIsOver = false;
        OnGameStarted();
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = "Score " + score.ToString();
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
            case PageState.MainMenu:
                startMenu.SetActive(true);
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
        scoreText.text = "Score 0";
        SetPageState(PageState.Start);
    }

    void StartGame()
    {

        // *** We need to hide the main menu here ***

        // When user presses play button, we change
        // to the countdown screen
        SetPageState(PageState.GameCountdown);
    }   
}
