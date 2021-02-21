using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHead : MonoBehaviour
{
	[SerializeField]
	private Hammer hammer;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("PushableBloc")) hammer.TouchCrate();
		if (collision.gameObject.CompareTag("Ground")) hammer.HeadTouchGround();
		
		if (!collision.gameObject.CompareTag("Player")) return;
		if (!hammer.active) return;
		
		collision.gameObject.GetComponent<PlayerController>().KillHammer(
			-collision.GetContact(0).normal);
		GetComponent<Collider2D>().isTrigger = true;
	}
}
