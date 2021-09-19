using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float thrustSpeed;
    [SerializeField] float MaxSpeed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunPos;
    [SerializeField] ParticleSystem thrustEffect;
    [SerializeField] AudioClip thrustSFX;
    [SerializeField] AudioClip fireSFX;
    [SerializeField] GameObject BoomVFX;
    AudioSource audioSource;
    Rigidbody rb;
    Camera mainCam;
    float horizontalInput;
    float verticalInput;
    float timer;
    int lives = 3;

    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShipControlls();
        PlayParticles();
    }

    private void Update()
    {
        Shoot();
        CheckPosition();
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

    private void Shoot()
    {
        timer += Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && timer > 0.1f)
        {
            audioSource.PlayOneShot(fireSFX);
            Instantiate(bulletPrefab, new Vector2(gunPos.position.x, gunPos.position.y), gunPos.rotation);
            timer = 0;
        }
    }

    private void ShipControlls()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        rb.freezeRotation = true;

        transform.Rotate(0, 0, -horizontalInput * rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.up * verticalInput * thrustSpeed * Time.deltaTime);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed), Mathf.Clamp(rb.velocity.y, -MaxSpeed, MaxSpeed));
        rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            lives--;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            if (lives == 0)
            {
                Instantiate(BoomVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            GameManager.instance.UpdateLives();
            Invoke("ActivateCollider", 2);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.tag == "Enemy")
    //    {
    //        print("!!!");
    //        lives--;
    //        gameObject.GetComponent<SphereCollider>().enabled = false;
    //        if (lives == 0)
    //        {
    //            Destroy(gameObject);
    //        }
    //        GameManager.instance.UpdateLives();
    //        Invoke("ActivateCollider", 2);
    //    }
    //}

    void ActivateCollider()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    private void CheckPosition()
    {

        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;

        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        if (transform.position.x > sceneRightEdge)
        {
            transform.position = new Vector2(sceneLeftEdge, transform.position.y);
        }
        if (transform.position.x < sceneLeftEdge)
        {
            transform.position = new Vector2(sceneRightEdge, transform.position.y);
        }
        if (transform.position.y > sceneTopEdge)
        {
            transform.position = new Vector2(transform.position.x, sceneBottomEdge);
        }
        if (transform.position.y < sceneBottomEdge)
        {
            transform.position = new Vector2(transform.position.x, sceneTopEdge);
        }

    }
}
