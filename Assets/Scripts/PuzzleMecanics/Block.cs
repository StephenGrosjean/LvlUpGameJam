using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock
{
	public readonly Transform Transform;
	public readonly BoxCollider2D Collider;
	public readonly Rigidbody2D Rigidbody;
	public readonly FixedJoint2D Joint;

	public PushableBlock(GameObject newGo)
	{
		Transform = newGo.transform;
		Collider = newGo.GetComponent<BoxCollider2D>();
		Rigidbody = newGo.GetComponent<Rigidbody2D>();
		Joint = newGo.GetComponent<FixedJoint2D>();
	}
}
