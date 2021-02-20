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
            MovementPlaceholder movementPlaceholder = collider.GetComponentInParent<MovementPlaceholder>();

            movementPlaceholder.TeleportPlayer(target.position, gravityDirection);
        }
    }
}
