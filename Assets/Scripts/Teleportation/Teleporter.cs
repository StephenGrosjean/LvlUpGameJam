using System;
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

    private Rigidbody2D playerRb;

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (!other.CompareTag(playerTag)) return;

		var transform1 = other.transform;
		playerRb       = transform1.GetComponent<Rigidbody2D>();

		transform1.position         = target.position;
		transform1.eulerAngles = RotatePlayer(gravityDirection);
	}

    private Vector3 RotatePlayer(Gravity gravityDir)
    {
		Vector3 rotation;
		switch (gravityDir)
		{
			case Gravity.UP:
			{
				if (Physics2D.gravity.x != 0)
				{
					var velocity = playerRb.velocity;
					velocity = new Vector2(velocity.y, velocity.x);
					playerRb.velocity = velocity;
				}

				rotation = new Vector3(0, 0, 180);
				Physics2D.gravity = new Vector3(0, 9.81f, 0);
				break;
			}
			case Gravity.DOWN:
			{
				if (Physics2D.gravity.x != 0)
				{
					var velocity = playerRb.velocity;
					velocity = new Vector2(velocity.y, velocity.x);
					playerRb.velocity = velocity;
				}
				
				rotation = new Vector3(0, 0, 0);
				Physics2D.gravity = new Vector3(0, -9.81f, 0);
				break;
			}
			case Gravity.LEFT:
			{
				if (Physics2D.gravity.y != 0)
				{
					var velocity = playerRb.velocity;
					velocity = new Vector2(velocity.y, velocity.x);
					playerRb.velocity = velocity;
				}
				
				rotation = new Vector3(0, 0, 270);
				Physics2D.gravity = new Vector3(-9.81f, 0, 0);
				break;
			}
			case Gravity.RIGHT:
			{
				if (Physics2D.gravity.y != 0)
				{
					var velocity = playerRb.velocity;
					velocity = new Vector2(velocity.y, velocity.x);
					playerRb.velocity = velocity;
				}
				
				rotation = new Vector3(0, 0, 90);
				Physics2D.gravity = new Vector3(9.81f, 0, 0);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException(
					nameof(gravityDir), gravityDir, null);
		}

		return rotation;
    }

}
