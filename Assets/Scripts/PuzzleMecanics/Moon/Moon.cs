using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Cinemachine;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] private GameObject moonSpriteRenderer;
    //[SerializeField] private Transform cameraTransform;
    //[SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject moonSprites;
    [SerializeField] private float collisionForces;

    [SerializeField] Vector3 cameraPosition = new Vector3(6,8, 10);

    private List<Rigidbody2D> moonRigidbody2Ds = new List<Rigidbody2D>();
    private List<Collider2D> moonColliders = new List<Collider2D>();
    private List<SpriteRenderer> moonSpriteRenderers = new List<SpriteRenderer>();


    [SerializeField] private float layerFadeOut = 0.01f;
    [SerializeField] private SpriteRenderer fadeOutSpriteRenderer;

    public bool destroyed = false;

    [SerializeField] private float timeBeforeMoonExplodes = 5f;

    private Transform cameraTransform;

    private float timer;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        transform.parent = cameraTransform;
        moonRigidbody2Ds.AddRange(moonSprites.GetComponentsInChildren<Rigidbody2D>());
        moonColliders.AddRange(moonSprites.GetComponentsInChildren<Collider2D>());
        moonSpriteRenderers.AddRange(moonSprites.GetComponentsInChildren<SpriteRenderer>());

        fadeOutSpriteRenderer.transform.parent = cameraTransform;
        fadeOutSpriteRenderer.transform.position = cameraTransform.position + Vector3.forward * 10;

        transform.localPosition = cameraPosition;

    }
    private void LateUpdate()
    {

        
        cameraTransform.localPosition = cameraPosition;
        if (!destroyed)
        {
            //Camera.main.transform.position = new Vector3(playerTransform.position.x, cameraTransform.position.y,
            //cameraTransform.position.z);
            // transform.position = new Vector3(cameraTransform.transform.position.x + cameraPosition.x, cameraTransform.transform.position.y + cameraPosition.y, 0);
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
                transform.parent = null;
                foreach (Rigidbody2D currentRigidbody in moonRigidbody2Ds)
                {
                    currentRigidbody.bodyType = RigidbodyType2D.Dynamic;
                    currentRigidbody.AddForce(new Vector2(Random.Range(-collisionForces, collisionForces), Random.Range(-collisionForces, collisionForces)));
                }

                foreach (Collider2D colliders in moonColliders)
                {
                    colliders.enabled = true;
                }
                foreach (SpriteRenderer sprites in moonSpriteRenderers)
                {
                    sprites.sortingLayerName = "Player";
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
