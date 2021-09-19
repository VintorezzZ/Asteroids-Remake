using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    Camera mainCam;
    float timer;
    [SerializeField] private Transform gunPos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] float maxSpeed = 3;
    [SerializeField] AudioClip fireSFX;
    [SerializeField] AudioClip bigBoomSFX;
    [SerializeField] GameObject BoomVFX;
    AudioSource audioSource;
    GameObject player;
    Rigidbody rb;
    void Start()
    {
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector2(1 , Random.Range(-maxSpeed, maxSpeed));

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckPosition();

        Shoot();

        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        if (player != null) 
            transform.LookAt(player.transform.position, Vector3.forward);
    }

    private void Shoot()
    {
        timer += Time.deltaTime;
        if (timer > 1.5f) 
        {
            audioSource.PlayOneShot(fireSFX);
            timer = 0;
            Instantiate(bulletPrefab, new Vector2(gunPos.position.x, gunPos.position.y), gunPos.rotation);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.AddPoints(1000);
            Instantiate(BoomVFX, transform.position, Quaternion.identity);
            AudioManager.instance.PlayBoomSFX(bigBoomSFX);
            Destroy(this.gameObject);
        }
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
