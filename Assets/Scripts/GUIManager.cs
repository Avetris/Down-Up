using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    enum GameStatus{STOPPED,TUTORIAL,PLAYING}

    public BallMovement _playerBall;

    [Header("Menu GUI")]
    // Game GUI
    [InspectorName("Menu Panel")]
    public GameObject _menuPanelGO;

    [Header("Game GUI")]
    // Game GUI
    [InspectorName("Game Score GUI")]
    public TextMeshProUGUI _gameScoreText;
    [InspectorName("Game Panel")]
    public GameObject _gamePanelGO;
    [InspectorName("Tutorial Panel")]
    public GameObject _tutorialPanelGO;


    // GameOver GUI
    [Header("Game Over GUI")]
    [InspectorName("Best Score GUI")]
    public TextMeshProUGUI _bestText;
    [InspectorName("Last Score GUI")]
    public TextMeshProUGUI _lastScoreText;
    [InspectorName("Game Over Panel")]
    public GameObject _gameOverPanelGO;

    private GameStatus _gameStatus = GameStatus.STOPPED;

    private void Start()
    {
        ShowMenu();
    }

    private void Update()
    {
        if(_gameStatus == GameStatus.TUTORIAL && Input.GetMouseButtonDown(0))
        {
            EndTutorial();
        }
    }

    public void ShowMenu()
    {
        _menuPanelGO.SetActive(true);
        _gamePanelGO.SetActive(false);
        _tutorialPanelGO.SetActive(false);
        _gameOverPanelGO.SetActive(false);
    }

    public bool IsStopped()
    {
        return _gameStatus != GameStatus.PLAYING;
    }

    public void SetScore(int score)
    {
        _gameScoreText.text = score.ToString();
    }

    public void EndTutorial()
    {
        _gameStatus = GameStatus.PLAYING;
        _playerBall.StartGame();
    }
    
    public void StartGame()
    {
        _menuPanelGO.SetActive(false);
        _gameOverPanelGO.SetActive(false);
        _tutorialPanelGO.SetActive(true);
        _gamePanelGO.SetActive(true);
        SetScore(0);
        _gameStatus = GameStatus.TUTORIAL;
        _playerBall.ResetGame();
    }

    public void ShowGameOver(int puntuation)
    {
        _gameStatus = GameStatus.STOPPED;
        _gamePanelGO.SetActive(false);
        _gameOverPanelGO.SetActive(true);
        _lastScoreText.text = puntuation.ToString();
    }

    public void ShowLeaderboard()
    {

    }
}
