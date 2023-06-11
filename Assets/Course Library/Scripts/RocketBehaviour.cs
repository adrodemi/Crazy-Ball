using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    private Transform target;
    private float speed = 16f;
    private bool homing;
    private float rocketStrength = 16f;
    private float aliveTimer = 6f;
    void Update()
    {
        if (homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }
        else if (target == null)
            Destroy(gameObject);
    }
    public void Fire(Transform newTarget)
    {
        target = newTarget;
        homing = true;
        Destroy(target, aliveTimer);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (target != null)
        {
            if (collision.gameObject.CompareTag(target.tag))
            {
                Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.contacts[0].normal;
                targetRb.AddForce(away * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }
}