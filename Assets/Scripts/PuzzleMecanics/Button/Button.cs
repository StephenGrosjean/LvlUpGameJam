using System;
using UnityEngine;

public class Button : MonoBehaviour
{
	[SerializeField] private ActivatableObject target;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.collider.CompareTag("Stopper")) return;
		
		spriteRenderer.color = Color.green;
		target.Enabled = true;
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (!other.collider.CompareTag("Stopper")) return;
		
		spriteRenderer.color = Color.red;
		target.Enabled = false;
	}
}
