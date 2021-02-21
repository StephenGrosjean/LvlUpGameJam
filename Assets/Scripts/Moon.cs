using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] private GameObject moonSpriteRenderer;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject moonSprites;
    [SerializeField] private float collisionForces;

    [SerializeField] Vector2 cameraPosition = new Vector2(2,3);

    private List<Rigidbody2D> moonRigidbody2Ds = new List<Rigidbody2D>();
    private List<Collider2D> moonColliders = new List<Collider2D>();

    private bool destroyed = false;

    private void Start()
    {
        moonRigidbody2Ds.AddRange(moonSprites.GetComponentsInChildren<Rigidbody2D>());
        moonColliders.AddRange(moonSprites.GetComponentsInChildren<Collider2D>());
    }
    private void Update()
    {
        if (!destroyed)
        {
            cameraTransform.position = new Vector3(playerTransform.position.x, cameraTransform.position.y,
                cameraTransform.position.z);
            transform.position = new Vector2(cameraTransform.position.x + cameraPosition.x,
                cameraTransform.position.y + cameraPosition.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!destroyed)
        {
            if (collider.tag == "Wall")
            {
                foreach (Rigidbody2D currentRigidbody in moonRigidbody2Ds)
                {
                    currentRigidbody.bodyType = RigidbodyType2D.Dynamic;
                    currentRigidbody.AddForce(new Vector2(Random.Range(-collisionForces, collisionForces), Random.Range(-collisionForces, collisionForces)));
                }

                foreach (Collider2D colliders in moonColliders)
                {
                    colliders.enabled = true;
                }

                destroyed = true;
                //moonSpriteRenderer.SetActive(false);
            }
        }


    }

}
