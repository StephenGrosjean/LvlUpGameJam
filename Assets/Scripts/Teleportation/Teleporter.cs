using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    public enum Gravity
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [SerializeField] private Transform target;
    [SerializeField] private Gravity gravityDirection;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == playerTag)
        {

            collider.transform.position = target.position;

            collider.transform.eulerAngles = RotatePlayer(gravityDirection);
        }
    }

    public Vector3 RotatePlayer(Teleporter.Gravity gravityDirection)
    {

        Vector3 rotation = new Vector3();

        if (gravityDirection == Teleporter.Gravity.UP)
        {
            rotation = new Vector3(0, 0, 180);
            Physics2D.gravity = new Vector3(0, 9.81f, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.DOWN)
        {
            rotation = new Vector3(0, 0, 0);
            Physics2D.gravity = new Vector3(0, -9.81f, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.LEFT)
        {
            rotation = new Vector3(0, 0, 270);
            Physics2D.gravity = new Vector3(-9.81f, 0, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.RIGHT)
        {
            rotation = new Vector3(0, 0, 90);
            Physics2D.gravity = new Vector3(9.81f, 0, 0);
        }

        return rotation;
    }

}
