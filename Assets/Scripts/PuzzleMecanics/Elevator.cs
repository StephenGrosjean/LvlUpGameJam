using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] bool loop = false;
    [SerializeField] bool active = false;
    [SerializeField] bool toEnd = true;

    [SerializeField] private Transform platform;
    [SerializeField] private Transform firstPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float elevatorSpeed;
    [SerializeField] private float detectionDist;

    private Vector3 direction;

    public void ActiveElevator()
    {
        active = true;
    }

    void Start() {
        direction = (endPos.position - firstPos.position).normalized;
        platform.position = firstPos.position;
    }

    void Update() {
        Debug.Log(Vector3.Distance(platform.transform.position, endPos.position));
        if (active)
        {
            if (toEnd)
            {
                platform.position += direction * elevatorSpeed;
                if (Vector3.Distance(platform.transform.position, endPos.position) < detectionDist)
                {
                    toEnd = false;
                    if (!loop)
                    {
                        active = false;
                    }
                }
            } else
            {
                platform.position -= direction * elevatorSpeed;
                if (Vector3.Distance(platform.transform.position, firstPos.position) < detectionDist)
                {
                    toEnd = true;
                    if (!loop)
                    {
                        active = false;
                    }
                }
            }
        }
    }
}
