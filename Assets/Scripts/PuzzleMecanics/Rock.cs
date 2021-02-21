using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float minVelocity;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Player")) return;
		other.gameObject.GetComponent<PlayerController>().KillRock();
		GetComponent<Collider2D>().isTrigger = true;
	}
}
