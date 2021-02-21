using UnityEngine;

public class ContactKill : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;

		other.GetComponent<PlayerController>().Kill();
		other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
}
