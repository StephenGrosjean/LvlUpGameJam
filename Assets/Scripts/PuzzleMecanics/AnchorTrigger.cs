using System;
using Cinemachine;
using UnityEngine;

public class AnchorTrigger : MonoBehaviour
{
	[SerializeField] private Transform anchor;
	[SerializeField] private float lensSize;

	private CameraRelay relay;
	private Transform playerPos;

	protected virtual void Start()
	{
		relay = FindObjectOfType<CameraRelay>();
		playerPos = FindObjectOfType<PlayerController>().transform;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;
		StartEffect();
	}

	protected virtual void StartEffect()
	{
        relay.SetLensSize(lensSize);
        relay.SetFollow(anchor ? anchor : playerPos);
	}
}
