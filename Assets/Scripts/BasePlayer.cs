using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
    public abstract class BasePlayer : VisibleObject
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

        [SerializeField] protected AudioClip fireSFX;
        [SerializeField] private int bulletPhysicsLayer;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
        }

        public virtual void Init()
        {
            
        }
        public virtual void Init(Player player)
        {
            Init();
            this.player = player;
        }

        protected virtual void Update()
        {
            if(CanShoot())
                Shoot();
            
            CheckPosition();
        }

        private bool CanShoot()
        {
            if(GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused || !GameManager.Instance.IsGameStarted)
                return false;

            return true;
        }

        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void Shoot()
        {
            if (lastShootTime + shootDelay <= Time.time)
            {
                audioSource.PlayOneShot(fireSFX);
                CreateBullet();
                lastShootTime = Time.time;
            }
        }

        private void CreateBullet()
        {
            var bullet = PoolManager.Get(PoolType.Bullet);
            bullet.gameObject.SetActive(true);
            bullet.transform.position = new Vector2(gunPos.position.x, gunPos.position.y);
            bullet.transform.rotation = gunPos.rotation;
            var b = bullet.GetComponent<Bullet>();
            b.gameObject.layer = bulletPhysicsLayer;
            b.owner = this;
            b.Init();
        }

        protected void SpawnExplosionVFX(AudioClip audioClip)
        {
            var explosion = PoolManager.Get(PoolType.Explosion_vfx).transform;
            explosion.gameObject.SetActive(true);
            explosion.position = transform.position;
            explosion.rotation = Quaternion.identity;
            
            if(audioClip)
                SoundManager.Instance.PlayBoomSFX(audioClip);
        }

        protected virtual void Die(AudioClip audioClip , float returningTime)
        {
            SpawnExplosionVFX(audioClip);
            Invoke(nameof(ReturnToPool), returningTime);
        }

        private void ReturnToPool()
        {
            rb.velocity = Vector3.zero;
            GameManager.Instance.aliveEntities.Remove(GetComponent<PoolItem>());
            PoolManager.Return(GetComponent<PoolItem>());
        }
    }
}