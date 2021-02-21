using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DupliEffect : AnchorTrigger
{
    private PlayerController player;
    [SerializeField] private Animator puzzleAnimator;
    [SerializeField] private bool starting = true;
    [SerializeField] private float animDelay = 3.0f;
    protected override void Start() {
        base.Start();
        player = FindObjectOfType<PlayerController>();
    }

    protected override void StartEffect() {
        base.StartEffect();
        if (!puzzleAnimator.GetBool("Duplicate"))
        {
            puzzleAnimator.SetBool("Duplicate", starting);
            player.enabled = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StartCoroutine(StopEffect());
        }
    }

    IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(animDelay);
        player.enabled = true;
    }
}
