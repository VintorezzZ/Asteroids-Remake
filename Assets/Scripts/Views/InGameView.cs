using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Views
{
    public class InGameView : View
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private List<GameObject> livesList;
        public override void Initialize()
        {
            EventHub.scoreChanged += UpdateScore;
            EventHub.healthChanged += UpdateLivesObjects;
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
            foreach (var life in livesList)
            {
                life.SetActive(true);
            }
            
            scoreText.text = "0";
        }

        private void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }
        
        private void UpdateLivesObjects(int livesCount)
        {
            livesList[livesCount].SetActive(false);
        }

        private void OnDestroy()
        {
            EventHub.scoreChanged -= UpdateScore;
            EventHub.healthChanged -= UpdateLivesObjects;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
