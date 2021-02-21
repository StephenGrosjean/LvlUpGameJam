using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    private List<SpriteRenderer> moonSpriteRenderers = new List<SpriteRenderer>();


    [SerializeField] private float layerFadeOut = 0.01f;
    [SerializeField] private SpriteRenderer fadeOutSpriteRenderer;

    private bool destroyed = false;

    [SerializeField] private float timeBeforeMoonExplodes = 5f;

    private float timer;

    private void Start()
    {
        moonRigidbody2Ds.AddRange(moonSprites.GetComponentsInChildren<Rigidbody2D>());
        moonColliders.AddRange(moonSprites.GetComponentsInChildren<Collider2D>());
        moonSpriteRenderers.AddRange(moonSprites.GetComponentsInChildren<SpriteRenderer>());
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

    private void FixedUpdate()
    {
        if (destroyed)
        {
            FadeOut();
            timer += Time.deltaTime;
        }

        if (timer > timeBeforeMoonExplodes)
        {
            FadeOutMoon();
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

                timer = 0;

                destroyed = true;
                //moonSpriteRenderer.SetActive(false);
            }
        }


    }


    void FadeOutMoon()
    {
        foreach (SpriteRenderer sprites in moonSpriteRenderers)
        {
            sprites.color -= new Color(0,0,0,layerFadeOut);

            if (sprites.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

    }

    void FadeOut()
    {
        fadeOutSpriteRenderer.color += new Color(0,0,0,layerFadeOut);
    }

}
