using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private ActivatableObject activatable;
    [SerializeField] private TargetJoint2D targetJoint;

    private void Start()
    {
        targetJoint.autoConfigureTarget = false;
    }

    private void Update()
    {
        targetJoint.enabled = !activatable.Enabled;
    }
}
