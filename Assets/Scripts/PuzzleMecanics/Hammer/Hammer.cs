using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private Animator animator;
    private Animation animation;

    [SerializeField] private float maxAngle;
    [SerializeField] private float rotationSpeed;
    private float currentValue;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] bool active = true;
    [SerializeField] bool toRight = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (toRight)
            {
                currentValue += rotationSpeed;
                if (currentValue >= 1.0f)
                {
                    toRight = false;
                }
            }
            else
            {
                currentValue -= rotationSpeed;
                if (currentValue <= 0.0f)
                {
                    toRight = true;
                }
            }
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-maxAngle, maxAngle, curve.Evaluate(currentValue)));
        }
    }

    public void TouchCase()
    {
        animator.enabled = true;
        active = false;
        animator.SetTrigger("Touch");
    }
}
