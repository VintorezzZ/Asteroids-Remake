using UnityEngine;

namespace DefaultNamespace
{
    public static class LevelSettings
    {
        private static float _sceneRightEdge;
        private static float _sceneLeftEdge;
        private static float _sceneTopEdge;
        private static float _sceneBottomEdge;
        
        public static float SceneRightEdge => _sceneRightEdge;
        public static float SceneLeftEdge => _sceneLeftEdge;
        public static float SceneTopEdge => _sceneTopEdge;
        public static float SceneBottomEdge => _sceneBottomEdge;

        public static void CalculateScreenBounds(Camera mainCamera)
        {
            float sceneWidth = mainCamera.orthographicSize * 2 * mainCamera.aspect;
            float sceneHeight = mainCamera.orthographicSize * 2;
            
            _sceneRightEdge = sceneWidth / 2;
            _sceneLeftEdge = _sceneRightEdge * -1;
            _sceneTopEdge = sceneHeight / 2;
            _sceneBottomEdge = _sceneTopEdge * -1;
        }
    }
}