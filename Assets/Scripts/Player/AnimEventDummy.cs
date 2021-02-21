using System;
using UnityEngine;

public class AnimEventDummy : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerController playerController;

    private void Start()
    {
        playerTransform = transform.parent.transform;
        playerController = playerTransform.GetComponent<PlayerController>();
    }

    public void PlayStepGrass()
    {
        SoundController.instance.PlayFX(FX.stepGrass);
    }

    public void SetState(int state)
    {
        playerController.SetState(state);
    }
}
