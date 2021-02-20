using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peaks : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<DeathManager>().KillPeaks();
        }
    }
}