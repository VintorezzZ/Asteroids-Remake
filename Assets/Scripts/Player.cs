using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Player : BasePlayer
{
    [SerializeField] float thrustSpeed;
    [SerializeField] ParticleSystem thrustEffect;
    [SerializeField] AudioClip thrustSFX;
    float horizontalInput;
    float verticalInput;
    int lives = 3;

    private Collider _collider;

    public override void Init()
    {
        base.Init();
        
        _collider = GetComponent<SphereCollider>();
    }

    protected override void FixedUpdate()
    {
        ShipControlls();
    }

    private void PlayParticles()
    {
        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            thrustEffect.Play();
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(thrustSFX);
            }            
        }
        
        if (Input.GetKeyUp("up") || Input.GetKeyUp("w"))
        {
            thrustEffect.Stop();
            audioSource.Stop();
        }
    }

    protected override void Shoot()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            base.Shoot();
        }
    }

    private void ShipControlls()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        rb.freezeRotation = true;

        transform.Rotate(0, 0, -horizontalInput * rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.up * verticalInput * thrustSpeed * Time.deltaTime);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        rb.freezeRotation = false;
        
        PlayParticles();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_collider.enabled/*other.gameObject.TryGetComponent(out EnemyShip enemy) || other.gameObject.TryGetComponent(out Asteroid asteroid)*/)
        {
            lives--;
            _collider.enabled = false;
            
            if (lives == 0)
            {
                Instantiate(boomVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            
            GameManager.instance.UpdateLives(lives);
            Invoke("ActivateCollider", 2);
        }
    }

    void ActivateCollider()
    {
        _collider.enabled = true;
    }
}
