using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public abstract class BasePlayer : MonoBehaviour
    {
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected Transform gunPos;
        [SerializeField] protected float shootDelay = .1f;
        protected float lastShootTime;
        protected AudioSource audioSource;
        protected Rigidbody rb;
        protected Player player;
        
        [SerializeField] protected float rotationSpeed;
        [FormerlySerializedAs("MaxSpeed")] [SerializeField] protected float maxSpeed;
        [SerializeField] protected float maxRotation;


        protected float screenOffset = 0f;
        protected float sceneRightEdge;
        protected float sceneLeftEdge;
        protected float sceneTopEdge;
        protected float sceneBottomEdge;

        [SerializeField] protected AudioClip fireSFX;
        [FormerlySerializedAs("BoomVFX")] [SerializeField] protected GameObject boomVFX;

        
        public virtual void Init()
        {
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();

            GetScreenBounds();
        }
        public virtual void Init(Player player)
        {
            Init();
            this.player = player;
            
            GetScreenBounds();
        }

        protected virtual void Update()
        {
            Shoot();
            CheckPosition();
        }
        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void Shoot()
        {
            if (lastShootTime + shootDelay <= Time.time)
            {
                audioSource.PlayOneShot(fireSFX);
                Instantiate(bulletPrefab, new Vector2(gunPos.position.x, gunPos.position.y), gunPos.rotation);
                lastShootTime = Time.time;
            }
        }
        private void GetScreenBounds()
        {
            sceneRightEdge = LevelSettings.SceneRightEdge;
            sceneLeftEdge = LevelSettings.SceneLeftEdge;
            sceneTopEdge = LevelSettings.SceneTopEdge;
            sceneBottomEdge = LevelSettings.SceneBottomEdge;
        }

        protected void CheckPosition()
        {
            if (transform.position.x > sceneRightEdge + screenOffset)
            {
                transform.position = new Vector2(sceneLeftEdge - screenOffset, transform.position.y);
            }

            if (transform.position.x < sceneLeftEdge - screenOffset)
            {
                transform.position = new Vector2(sceneRightEdge + screenOffset, transform.position.y);
            }

            if (transform.position.y > sceneTopEdge + screenOffset)
            {
                transform.position = new Vector2(transform.position.x, sceneBottomEdge - screenOffset);
            }

            if (transform.position.y < sceneBottomEdge - screenOffset)
            {
                transform.position = new Vector2(transform.position.x, sceneTopEdge + screenOffset);
            }
        }
    }
}