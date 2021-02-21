using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos;
    [SerializeField] private float parallaxEffect;
    private GameObject cam;

    [SerializeField] private float minPos, maxPos;
    private bool canMove;
    private Transform player;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minPos, -100, 0), new Vector3(minPos, 100,0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(maxPos, -100, 0), new Vector3(maxPos, 100, 0));
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position.x;
        cam = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        canMove = player.position.x >= minPos && player.position.x <= maxPos;
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        float dist = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
