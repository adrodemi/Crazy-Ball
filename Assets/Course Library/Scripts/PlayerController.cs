using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float speed = 6f;
    public bool hasPowerup = false;
    private float powerUpStrength = 16f;
    public GameObject powerupIndicator;

    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerUpCountdown;

    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;

    bool smashing = false;
    float floorY;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        
        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

        if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
        }
        if (transform.position.y < -4f)
        {
            gameManager.DefeatGame();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(CountDownRoutine());

            if (powerUpCountdown != null)
            {
                StopCoroutine(powerUpCountdown);
            }
            powerUpCountdown = StartCoroutine(CountDownRoutine());
        }
    }
    IEnumerator CountDownRoutine()
    {
        yield return new WaitForSeconds(6f);
        hasPowerup = false;
        currentPowerUp = PowerUpType.None;
        powerupIndicator.SetActive(false);
    }
    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();
        floorY = transform.position.y;
        float jumpTime = Time.time + hangTime;
        while (Time.time < jumpTime)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }
        while (transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                    transform.position, explosionRadius, 0f, ForceMode.Impulse);
            }
        }
        smashing = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.PushBack)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + currentPowerUp.ToString());
        }
    }
    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }
}