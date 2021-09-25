using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Asteroid : BasePlayer, IPoolObservable
{
    [SerializeField] Asteroid subAsteroid;
    [SerializeField] int points;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    [SerializeField] AudioClip bigBoomSFX;
    [SerializeField] AudioClip smallBoomSFX;
    AudioClip audioClip;

    protected override void Awake()
    {
        base.Awake();
        
        maxRotation = 25f;
        maxSpeed = 2f;
        screenOffset = 1f;
    }

    public override void Init()
    {
        base.Init();
        
        rotationX = Random.Range(-maxRotation, maxRotation);
        rotationY = Random.Range(-maxRotation, maxRotation);
        rotationZ = Random.Range(-maxRotation, maxRotation);

        rb.velocity = new Vector2(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));
    }

    protected override void Update()
    {
        CheckPosition();
        transform.Rotate(new Vector3(rotationX, rotationY, 0) * Time.deltaTime);
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            points = 125;
            audioClip = smallBoomSFX;
            if (subAsteroid != null)
            {
                CreateSubAsteroids();
                
                GameManager.Instance.aliveAsteroids.Remove(this);
                audioClip = bigBoomSFX;
                points = 75;
            }
            
            GameManager.Instance.AddPoints(points);
            Die(audioClip, .1f);
        }

        if (other.gameObject.TryGetComponent(out Player player))
        {
            audioClip = bigBoomSFX;
            if (subAsteroid != null)
            {
                GameManager.Instance.aliveAsteroids.Remove(this);
            }
            
            Die(audioClip, .1f);
        }
    }

    private void CreateSubAsteroids()
    {
        GameManager.Instance.spawner.SpawnAsteroids(2, PoolType.SmallAsteroid, transform.position);
    }

    public void OnReturnToPool()
    {
        
    }

    public void OnTakeFromPool()
    {
        
    }
}
