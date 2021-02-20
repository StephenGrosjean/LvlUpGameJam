using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer moonSpriteRenderer;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;

    [SerializeField] Vector2 cameraPosition = new Vector2(2,3);

   
    private void Update()
    {
        cameraTransform.position = new Vector3(playerTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
        transform.position = new Vector2(cameraTransform.position.x + cameraPosition.x, cameraTransform.position.y + cameraPosition.y);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Wall")
        {
            moonSpriteRenderer.enabled = false;
        }


    }

}
