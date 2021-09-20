using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticButtons : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.StartGame();
    }

    public void Unpause()
    {
        GameManager.instance.UnpauseGame();
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
