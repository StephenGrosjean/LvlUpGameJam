using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFall : ActivatableObject
{
    private Animator treeAnimator;

    void Start() {
        treeAnimator = GetComponent<Animator>();
    }

    protected void OnButtonActionDown()
    {
        treeAnimator.Play("Fall");
    }

    void Update() {
        if (Enabled)
        {
            OnButtonActionDown();
        }
    }
}
