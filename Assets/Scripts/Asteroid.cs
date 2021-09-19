using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float maxRotation = 25f;
    [SerializeField] float maxSpeed = 2;
    [SerializeField] GameObject subAsteroid;
    [SerializeField] int points;
    Camera mainCam;
    Rigidbody rb;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    AudioSource audioSource;
    [SerializeField] AudioClip bigBoomSFX;
    [SerializeField] AudioClip smallBoomSFX;
    AudioClip audioClip;
    [SerializeField] GameObject BoomVFX;

    private void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();

        rotationX = Random.Range(-maxRotation, maxRotation);
        rotationY = Random.Range(-maxRotation, maxRotation);
        rotationZ = Random.Range(-maxRotation, maxRotation);

        rb.velocity = new Vector2(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));

        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        CheckPosition();
        transform.Rotate(new Vector3(rotationX, rotationY, 0) * Time.deltaTime);
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            points = 125;
            audioClip = smallBoomSFX;
            if (subAsteroid != null)
            {
                Instantiate(subAsteroid, transform.position, Quaternion.identity);
                Instantiate(subAsteroid, transform.position, Quaternion.identity);
                GameManager.instance.aliveAsteroids.Remove(this.gameObject);
                audioClip = bigBoomSFX;
                points = 75;
            }
            AudioManager.instance.PlayBoomSFX(audioClip);
            Instantiate(BoomVFX, transform.position, Quaternion.identity);
            GameManager.instance.AddPoints(points);
            Destroy(gameObject, 0.1f);
        }

        if (other.gameObject.tag == "Player")
        {
            audioClip = bigBoomSFX;
            if (subAsteroid != null)
            {
                GameManager.instance.aliveAsteroids.Remove(this.gameObject);
            }
            AudioManager.instance.PlayBoomSFX(audioClip);
            Instantiate(BoomVFX, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.1f);
        }
    }

    private void CheckPosition()
    {

        float rockOffset = 1;

        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;

        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        if (transform.position.x > sceneRightEdge + rockOffset)
        {
            transform.position = new Vector2(sceneLeftEdge - rockOffset, transform.position.y);
        }

        if (transform.position.x < sceneLeftEdge - rockOffset)
        {
            transform.position = new Vector2(sceneRightEdge + rockOffset, transform.position.y);
        }

        if (transform.position.y > sceneTopEdge + rockOffset)
        {
            transform.position = new Vector2(transform.position.x, sceneBottomEdge - rockOffset);
        }

        if (transform.position.y < sceneBottomEdge - rockOffset)
        {
            transform.position = new Vector2(transform.position.x, sceneTopEdge + rockOffset);
        }

    }

}
