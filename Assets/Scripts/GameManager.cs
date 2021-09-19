using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> asteroidsList;
    public List<GameObject> aliveAsteroids;
    public List<GameObject> spawners;
    public List<GameObject> livesList;
    public GameObject enemyShip;
    Vector3 spawnPoint;
    int score;
    int lives = 3;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text scoreText;
    float timer;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
        SpawnAsteroids(5);
        Time.timeScale = 0;
    }

    void SpawnAsteroids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject asteroid = asteroidsList[Random.Range(0, asteroidsList.Count - 1)];
            spawnPoint = spawners[Random.Range(0, spawners.Count - 1)].transform.position;
            GameObject asteroidClon = Instantiate(asteroid, spawnPoint, Quaternion.identity);
            aliveAsteroids.Add(asteroidClon);
        }
    }
    
    void SpawnEnemyShip()
    {
        spawnPoint = spawners[Random.Range(0, spawners.Count - 1)].transform.position;
        Instantiate(enemyShip, spawnPoint, Quaternion.identity);
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

        timer += Time.deltaTime;
        if (timer > 30f)
        {
            SpawnEnemyShip();
            timer = 0;
        }
        
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    internal void GameOver()
    {
        gameOverPanel.SetActive(true);
        Cursor.visible = true;
    }

    internal void UpdateLives()
    {
        lives--;
        Destroy(livesList[lives]);
        if (lives == 0)
        {
            lives = 0;
            GameOver();
        }
    }
}
