using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<Asteroid> aliveAsteroids;
    public List<GameObject> spawners;
    public List<GameObject> livesList;
    public float spawnEnemyDelay = 30f;
    private float _lastSpawnEnemyTime;
    private int _score;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text scoreText;
    public Player player;
    private void Awake()
    {
        instance = this;
        LevelSettings.CalculateScreenBounds(Camera.main);
    }
    private void Start()
    {
        _score = 0;
        scoreText.text = _score.ToString();
        SpawnPlayer();
        SpawnAsteroids(5);
        Time.timeScale = 0;
        spawnEnemyDelay = 30f;
    }

    void SpawnAsteroids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var position = spawners[Random.Range(0, spawners.Count - 1)].transform.position;
            var asteroid = SpawnObject(PoolType.BigAsteroid, position, Quaternion.identity).GetComponent<Asteroid>();
            asteroid.Init();
            aliveAsteroids.Add(asteroid);
        }
    }

    private PoolItem SpawnObject(PoolType type, Vector3 position, Quaternion rotation)
    {
        var item = PoolManager.Get(type);
        item.gameObject.SetActive(true);
        item.transform.position = position;
        item.transform.rotation = rotation;
        return item;
    }

    void SpawnEnemyShip()
    {
        var spawnPoint = spawners[Random.Range(0, spawners.Count - 1)].transform.position;
        var ship = SpawnObject(PoolType.EnemyShip, spawnPoint, Quaternion.identity);
        ship.GetComponent<EnemyShip>().Init(player);
    }

    void SpawnPlayer()
    {
        player = Instantiate(player, Vector3.zero, Quaternion.identity);
        player.Init();
    }
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.8f);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 0;
                pausePanel.SetActive(true);
                Cursor.visible = true;
            }
        }

        if (aliveAsteroids.Count < 5) 
        {
            SpawnAsteroids(1);
        }

        if (_lastSpawnEnemyTime + 5 <= Time.time)
        {
            SpawnEnemyShip();
            _lastSpawnEnemyTime = Time.time;
        }
    }

    public void AddPoints(int points)
    {
        _score += points;
        scoreText.text = _score.ToString();
    }

    internal void GameOver()
    {
        gameOverPanel.SetActive(true);
        Cursor.visible = true;
    }

    internal void UpdateLives(int livesCount)
    {
        Destroy(livesList[livesCount]);
        if (livesCount == 0)
        {
            GameOver();
        }
    }
}
