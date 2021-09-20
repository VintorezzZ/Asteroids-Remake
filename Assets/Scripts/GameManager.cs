using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using EventHandler = DefaultNamespace.EventHandler;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<Asteroid> aliveAsteroids;
    public float spawnEnemyDelay = 30f;
    
    private float _lastEnemySpawnTime;
    private int _score;
    
    [HideInInspector] public Player player;
    
    private bool _gameOver = false;    
    private bool _gameStarted = false;
    private bool _gamePaused = false;
    public bool IsGameOver => _gameOver;
    public bool IsGameStarted => _gameStarted;
    public bool IsGamePaused => _gamePaused;

    [SerializeField] Spawner _spawner;

    private void Awake()
    {
        instance = this;
        LevelSettings.CalculateScreenBounds(Camera.main);
        _spawner = FindObjectOfType<Spawner>();
    }
    private void Start()
    {
        _score = 0;
        player = _spawner.SpawnPlayer();
        _spawner.SpawnAsteroids(5);
        Time.timeScale = 0;
        _gameOver = false;
    }
    
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.8f);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                if(_gameStarted)
                    UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (aliveAsteroids.Count < 5) 
        {
            _spawner.SpawnAsteroids(1);
        }

        if (_lastEnemySpawnTime + spawnEnemyDelay <= Time.time)
        {
            _spawner.SpawnEnemyShip();
            _lastEnemySpawnTime = Time.time;
        }
    }

    public void StartGame()
    {
        _gameStarted = true;
        UnpauseGame();
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        _gamePaused = false;
        EventHandler.OnGamePaused(false);   
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        _gamePaused = true;
        EventHandler.OnGamePaused(true);
    }

    public void AddPoints(int points)
    {
        _score += points;
        EventHandler.OnScoreChanged(_score);
    }

    internal void GameOver()
    {
        Cursor.visible = true;
        _gameOver = true;
        EventHandler.OnGameOvered();
    }

    internal void UpdateLives(int livesCount)
    {
        EventHandler.OnHealthChanged(livesCount);
        if (livesCount == 0)
        {
            GameOver();
        }
    }
}
