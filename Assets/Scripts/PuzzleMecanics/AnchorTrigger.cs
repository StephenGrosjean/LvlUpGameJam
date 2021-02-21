using System;
using Cinemachine;
using UnityEngine;

public class AnchorTrigger : MonoBehaviour
{
	[SerializeField] private Transform anchor;

	private CameraRelay relay;
	private Transform playerPos;

	private void Start()
	{
		relay = FindObjectOfType<CameraRelay>();
		playerPos = FindObjectOfType<PlayerController>().transform;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;

		relay.SetFollow(anchor ? anchor : playerPos);
		relay.SetLensSize(anchor ? -anchor.position.z : relay.GetLensSize());
	}
}
