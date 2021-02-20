using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacter : MonoBehaviour
{
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxisRaw("Horizontal") * 10.0f, GetComponent<Rigidbody2D>().velocity.y);
        cameraTransform.transform.position = new Vector3(transform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }
}
