using System;

namespace DefaultNamespace
{
    public static class EventHandler
    {
        public static event Action gameOvered;
        public static event Action<bool> gamePaused;
        public static event Action<int> scoreChanged;
        public static event Action<int> healthChanged;

        public static void OnGameOvered()
        {
            gameOvered?.Invoke();
        }
        
        public static void OnGamePaused(bool paused)
        {
            gamePaused?.Invoke(paused);
        }
        
        public static void OnScoreChanged(int score)
        {
            scoreChanged?.Invoke(score);
        }
        
        public static void OnHealthChanged(int health)
        {
            healthChanged?.Invoke(health);
        }
    }
}