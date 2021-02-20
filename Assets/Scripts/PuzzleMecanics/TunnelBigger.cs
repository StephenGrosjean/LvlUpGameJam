using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBigger : MonoBehaviour
{
    [SerializeField] private BoxCollider2D realRightCollider;
    [SerializeField] private BoxCollider2D realLeftCollider;
    [SerializeField] private BoxCollider2D falseRightCollider;
    [SerializeField] private BoxCollider2D falseLeftCollider;
    [SerializeField] private Vector3 realRightPos;
    [SerializeField] private Vector3 realLeftPos;
    [SerializeField] private Vector3 falseRightPos;
    [SerializeField] private Vector3 falseLeftPos;
    private Transform characterTransform;
    private bool realState = true;
    private bool lastPosReal = true;
    // Start is called before the first frame update
    void Start()
    {
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
        realRightPos = realRightCollider.transform.position + Vector3.right*2;
        realLeftPos = realLeftCollider.transform.position + Vector3.left * 2;
        falseRightPos = falseRightCollider.transform.position + Vector3.left * 2;
        falseLeftPos = falseLeftCollider.transform.position + Vector3.right * 2;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (realState) {
            if (realLeftCollider.OverlapPoint(characterTransform.position)) {
                characterTransform.position += Vector3.right * 2 - 20.0f * Vector3.up;
                Camera.main.transform.position -= 20.0f * Vector3.up;
                realState = false;
                Debug.Log("realLeftCollider");
            } else if (realRightCollider.OverlapPoint(characterTransform.position)) {
                characterTransform.position += Vector3.left * 2 - 20.0f * Vector3.up;
                Camera.main.transform.position -= 20.0f * Vector3.up;
                realState = false;
                Debug.Log("realRightCollider");
                Camera.main.orthographicSize -= 10;
                characterTransform.localScale -= 2.0f * Vector3.one;
            }
        } else {
            if (falseLeftCollider.OverlapPoint(characterTransform.position)) {
                characterTransform.position += Vector3.left * 2 + 20.0f * Vector3.up;
                Camera.main.transform.position += 20.0f * Vector3.up;
                realState = true;
                Debug.Log("falseLeftCollider");
            } else if (falseRightCollider.OverlapPoint(characterTransform.position)) {
                characterTransform.position += Vector3.right * 2 + 20.0f * Vector3.up;
                Camera.main.transform.position += 20.0f * Vector3.up;
                realState = true;
                Debug.Log("falseRightCollider");
                Camera.main.orthographicSize += 10;
                characterTransform.localScale += 2.0f * Vector3.one;
            }
        }
    }
}
