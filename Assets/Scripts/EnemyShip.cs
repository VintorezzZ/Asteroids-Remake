﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShip : BasePlayer
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
            GameManager.instance.AddPoints(1000);
            Instantiate(boomVFX, transform.position, Quaternion.identity);
            AudioManager.instance.PlayBoomSFX(bigBoomSFX);
            Destroy(gameObject);
        }
    }
}
