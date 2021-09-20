using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance;
        
        [SerializeField] GameObject startPanel;
        [SerializeField] GameObject pausePanel;
        [SerializeField] GameObject gameOverPanel;
        
        [SerializeField] Text scoreText;
        public List<GameObject> livesList;

        private void Awake()
        {
            Instance = this;
            
            EventHandler.gamePaused += ShowPausePanel;
            EventHandler.gameOvered += ShowGameOverPanel;
            EventHandler.scoreChanged += UpdateScore;
            EventHandler.healthChanged += UpdateLivesObjects;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if(arg0.buildIndex == 1)
            {
                SetupUi();
            }        
        }

        private void SetupUi()
        {
            gameOverPanel.SetActive(false);
            startPanel.SetActive(true);
            foreach (var life in livesList)
            {
                life.SetActive(true);
            }
        }

        private void Start()
        {
            scoreText.text = "0";
        }

        private void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }

        private void ShowGameOverPanel()
        {
            gameOverPanel.SetActive(true);
        }

        private void ShowPausePanel(bool pause)
        {
            if (pause)
            {
                pausePanel.SetActive(true);
            }
            else
            {
                pausePanel.SetActive(false);
                startPanel.SetActive(false);
            }
        }

        private void UpdateLivesObjects(int livesCount)
        {
            livesList[livesCount].SetActive(false);
        }
    }
}