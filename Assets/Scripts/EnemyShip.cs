using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShip : BasePlayer, IPoolObservable
{
    [SerializeField] AudioClip bigBoomSFX;
    
    public override void Init(Player player)
    {
        base.Init(player);
        
        rb.velocity = new Vector2(1 , Random.Range(-maxSpeed, maxSpeed));
        shootDelay = 1.5f;
        maxSpeed = 3f;
        
        audioSource.Play();
    }

    protected override void Update()
    {
        base.Update();
        
        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        if (player != null) 
            transform.LookAt(player.transform.position, Vector3.forward);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Bullet bullet) || collision.gameObject.TryGetComponent(out Player player))
        {
            GameManager.Instance.AddPoints(1000);
            Die(bigBoomSFX, 0f);
        }
    }

    public void OnReturnToPool()
    {
        
    }

    public void OnTakeFromPool()
    {
        
    }
}
