using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticButtons : MonoBehaviour
{
    [SerializeField] GameObject pausePanel; 
    [SerializeField] GameObject startPanel; 
    public void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    public void Unpause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
