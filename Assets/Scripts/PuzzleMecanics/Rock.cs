using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float minVelocity;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && body.velocity.magnitude >= minVelocity)
        {
            other.gameObject.GetComponent<DeathManager>().KillRock();
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
