using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Asteroid : BasePlayer
{
    [SerializeField] Asteroid subAsteroid;
    [SerializeField] int points;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    [SerializeField] AudioClip bigBoomSFX;
    [SerializeField] AudioClip smallBoomSFX;
    AudioClip audioClip;
    
    public override void Init()
    {
        base.Init();
        
        rotationX = Random.Range(-maxRotation, maxRotation);
        rotationY = Random.Range(-maxRotation, maxRotation);
        rotationZ = Random.Range(-maxRotation, maxRotation);

        rb.velocity = new Vector2(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));
        
        maxRotation = 25f;
        maxSpeed = 2f;
        screenOffset = 1f;
    }

    protected override void Update()
    {
        CheckPosition();
        transform.Rotate(new Vector3(rotationX, rotationY, 0) * Time.deltaTime);
    }

    private void StartDeath()
    {
        AudioManager.instance.PlayBoomSFX(audioClip);
        Instantiate(boomVFX, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.1f);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            points = 125;
            audioClip = smallBoomSFX;
            if (subAsteroid != null)
            {
                Instantiate(subAsteroid, transform.position, Quaternion.identity).Init();
                Instantiate(subAsteroid, transform.position, Quaternion.identity).Init();
                GameManager.instance.aliveAsteroids.Remove(this);
                audioClip = bigBoomSFX;
                points = 75;
            }
            
            GameManager.instance.AddPoints(points);
            StartDeath();
        }

        if (other.gameObject.TryGetComponent(out Player player))
        {
            audioClip = bigBoomSFX;
            if (subAsteroid != null)
            {
                GameManager.instance.aliveAsteroids.Remove(this);
            }
            
            StartDeath();
        }
    }
}
