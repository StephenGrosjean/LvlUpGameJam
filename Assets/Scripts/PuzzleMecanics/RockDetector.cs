using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDetector : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rock;

    void OnTriggerEnter2D(Collider2D other) {
        rock.gravityScale = 1;
    }
}
