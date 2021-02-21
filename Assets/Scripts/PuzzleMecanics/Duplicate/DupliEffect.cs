using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DupliEffect : AnchorTrigger
{
    [SerializeField] private Animator puzzleAnimator;
    [SerializeField] private bool starting = true;
    protected override void Start() {
        base.Start();

    }

    protected override void StartEffect() {
        base.StartEffect();
        puzzleAnimator.SetBool("Duplicate", starting);
    }
}
