using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MainMenuView : View
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button startButton;
        
        public override void Initialize()
        {
            exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
            startButton.onClick.AddListener(() =>
            {
                GameManager.Instance.StartGame();
                Hide();
                ViewManager.Show<InGameView>();
            });
        }
    }
}