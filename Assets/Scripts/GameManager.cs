using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventHandler = DefaultNamespace.EventHandler;

public class GameManager : SingletonBehaviour<GameManager>
{
    public List<PoolItem> aliveEntities;
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

    public Spawner spawner;

    private void Awake()
    {
        InitializeSingleton();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        
        LevelSettings.CalculateScreenBounds(Camera.main);
        spawner = FindObjectOfType<Spawner>();
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        if (arg0.buildIndex == 1)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.buildIndex == 1)
        {
            spawner = FindObjectOfType<Spawner>();
            player = spawner.SpawnPlayer();
            spawner.SpawnAsteroids(5, PoolType.BigAsteroid);
            _gameOver = false;
            Time.timeScale = 0;
            _score = 0;
            AddPoints(0);

            _score = 0;
            _gameOver = false;
        }
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

        if(IsGameOver)
            return;
        
        if (aliveAsteroids.Count < 5) 
        {
            spawner.SpawnAsteroids(1, PoolType.BigAsteroid);
        }

        if (_lastEnemySpawnTime + spawnEnemyDelay <= Time.time)
        {
            spawner.SpawnEnemyShip();
            _lastEnemySpawnTime = Time.time;
        }
    }

    public void StartGame()
    {
        _gameStarted = true;
        _lastEnemySpawnTime = Time.time;
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

    public void RestartGame()
    {
        aliveAsteroids.Clear();

        foreach (var item in aliveEntities)
        {
            item.ReturnToPool();
        }
        
        aliveEntities.Clear();
        
        SceneManager.UnloadSceneAsync(1);
        
        // foreach (var entity in aliveEntities)
        // {
        //     entity.ReturnToPool();
        // }
        //
        // spawner.SpawnPlayer();
    }
}
