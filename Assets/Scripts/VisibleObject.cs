using UnityEngine;

namespace DefaultNamespace
{
    public abstract class VisibleObject : MonoBehaviour
    {
        protected float screenOffset = 0f;

        protected void CheckPosition()
        {
            if (transform.position.x > LevelSettings.SceneRightEdge + screenOffset)
            {
                transform.position = new Vector2(LevelSettings.SceneLeftEdge - screenOffset, transform.position.y);
            }

            if (transform.position.x < LevelSettings.SceneLeftEdge - screenOffset)
            {
                transform.position = new Vector2(LevelSettings.SceneRightEdge + screenOffset, transform.position.y);
            }

            if (transform.position.y > LevelSettings.SceneTopEdge + screenOffset)
            {
                transform.position = new Vector2(transform.position.x, LevelSettings.SceneBottomEdge - screenOffset);
            }

            if (transform.position.y < LevelSettings.SceneBottomEdge - screenOffset)
            {
                transform.position = new Vector2(transform.position.x, LevelSettings.SceneTopEdge + screenOffset);
            }
        }
    }
}