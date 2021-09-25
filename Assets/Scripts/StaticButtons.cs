using UnityEngine;

public class StaticButtons : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void Unpause()
    {
        GameManager.Instance.UnpauseGame();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void ExitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
