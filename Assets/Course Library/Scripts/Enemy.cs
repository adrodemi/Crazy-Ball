using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    public float speed = 4f;
    public bool isBoss = false;
    public float spawnInterval;
    private float nextSpawn;
    public int miniEnemySpawnCount;
    private SpawnManager spawnManager;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        if (isBoss)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }

        if (transform.position.y < -2)
            Destroy(gameObject);
    }
}