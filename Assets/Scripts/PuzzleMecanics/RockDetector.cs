using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDetector : MonoBehaviour
{
    [SerializeField] private GameObject rock;

    void OnTriggerEnter2D(Collider2D other) {
        rock.SetActive(true);
    }
}
