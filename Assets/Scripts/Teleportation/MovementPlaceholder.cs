using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlaceholder : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playeRigidbody2D;
    [SerializeField] private float velocity;
    [SerializeField] private Transform cameraTransform;
    

    void FixedUpdate()
    {
        playeRigidbody2D.velocity = new Vector2(0, playeRigidbody2D.velocity.y);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playeRigidbody2D.velocity = new Vector2(velocity, playeRigidbody2D.velocity.y);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playeRigidbody2D.velocity = new Vector2(-velocity, playeRigidbody2D.velocity.y);
        }

        if (cameraTransform != null)
        {
            cameraTransform.position = new Vector3(transform.position.x, cameraTransform.position.y, cameraTransform.position.z);
        }
    }

    public void TeleportPlayer(Vector3 position, Teleporter.Gravity gravityDirection)
    {
        transform.position = position;

        if (gravityDirection == Teleporter.Gravity.UP)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            Physics2D.gravity = new Vector3(0, 9.81f, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.DOWN)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            Physics2D.gravity = new Vector3(0, -9.81f, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.LEFT)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            Physics2D.gravity = new Vector3(-9.81f, 0, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.RIGHT)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            Physics2D.gravity = new Vector3(9.81f, 0, 0);
        }
    }
}
