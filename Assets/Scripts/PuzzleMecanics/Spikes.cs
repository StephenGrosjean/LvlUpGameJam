using UnityEngine;

public class Spikes : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player")) other.GetComponent<PlayerController>().KillSpikes();
	}
}
