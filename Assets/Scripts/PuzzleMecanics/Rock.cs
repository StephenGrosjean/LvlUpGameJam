using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<DeathManager>().KillRock();
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
