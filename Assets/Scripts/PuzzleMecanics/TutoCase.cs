using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoCase : CaseButton
{
    [SerializeField] private Animator treeAnimator;
    protected override void OnButtonActionDown()
    {
        treeAnimator.Play("Fall");
    }
}
