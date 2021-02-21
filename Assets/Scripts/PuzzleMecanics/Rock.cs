using UnityEngine;

public class Rock : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Player")) return;
		other.gameObject.GetComponent<PlayerController>().KillRock();
		GetComponent<Collider2D>().isTrigger = true;
	}
}
