using System;
using System.Collections.Generic;
using UnityEngine;
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

        private void ShowPausePanel(bool show)
        {
            if (show)
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
            Destroy(livesList[livesCount]);
        }
    }
}