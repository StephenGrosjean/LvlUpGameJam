using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform scene;
    [SerializeField] private GameObject scenePrefab;

	public Transform GetRespawnPoint() { return respawnPoint; }

	public void ReloadScene()
    {
        var pos = scene.transform.position;
        Destroy(scene.gameObject);
        scene = Instantiate(scenePrefab, pos, Quaternion.identity).transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<PlayerController>().SetRespawnPoint(this);
    }
}
