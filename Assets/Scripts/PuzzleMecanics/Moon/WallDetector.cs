using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private SpriteRenderer moon;
    [SerializeField] private float layerFadeIn;
    private bool fadeId = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wall.SetActive(true);
            fadeId = true;
        }
    }

    void FixedUpdate() {
        if (fadeId)
        {
            FadeIn();
        }
    }

    void FadeIn()
    {
        moon.color -= new Color(0, 0, 0, layerFadeIn);
    }
}
