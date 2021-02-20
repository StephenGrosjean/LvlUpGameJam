using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    [Header("Anchors")]
    [SerializeField] private Transform leftAnchorPoint;
    [SerializeField] private Transform rightAnchorPoint;

    private Transform playerTransform;
    private Collider2D playerCollider2D;
    
    private void Start()
    {
        playerTransform = GetComponent<PlayerController>().gameObject.transform;
        playerCollider2D = GetComponent<PlayerController>().GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!Input.GetButton("Interact")) return;
        
        int layer = 1 << LayerMask.NameToLayer("Player");
        var hit = 
            Physics2D.CircleCast(leftAnchorPoint.position, 0.1f, Vector2.left, 0.2f, layer);
        if (hit)
        {
            Debug.Log("Can push block from left");
            return;
        }
        
        hit = 
            Physics2D.CircleCast(rightAnchorPoint.position, 0.1f, Vector2.right, 0.2f, layer);
        if (hit)
        {
            Debug.Log("Can push block from right");
        }
    }
}
