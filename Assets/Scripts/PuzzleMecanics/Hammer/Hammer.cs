using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
	private new Rigidbody2D rigidbody;

	[SerializeField] private float maxAngle;
	[SerializeField] private float rotationSpeed;
	private float currentValue;
	[SerializeField] private AnimationCurve curve;
	[SerializeField] public bool active = true;
	[SerializeField] private bool toRight = true;
	private bool headTouchGround;
	private bool handleTouchGround;

	// Start is called before the first frame update
	private void Start()
	{
		rigidbody             = GetComponentInChildren<Rigidbody2D>();
		rigidbody.isKinematic = true;
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		if (active)
		{
			if (toRight)
			{
				currentValue += rotationSpeed;
				if (currentValue >= 1.0f) { toRight = false; }
			}
			else
			{
				currentValue -= rotationSpeed;
				if (currentValue <= 0.0f) { toRight = true; }
			}
			transform.rotation = Quaternion.Euler(
				0, 0, Mathf.Lerp(-maxAngle, maxAngle, curve.Evaluate(currentValue)));
		}
		else
		{
			if (rigidbody.transform.rotation.eulerAngles.z < 100 ||
				rigidbody.transform.rotation.eulerAngles.z > 320)
			{
				rigidbody.transform.rotation =
					Quaternion.Euler(0, 0, rigidbody.transform.rotation.eulerAngles.z - 1);
			}
			else
			{
				if (handleTouchGround && headTouchGround)
				{
					rigidbody.isKinematic     = true;
					rigidbody.velocity        = Vector3.zero;
					rigidbody.angularVelocity = 0.0f;
				}
				else
				{
					rigidbody.isKinematic = false;
				}
			}
		}
	}

	public void TouchCrate() { active = false; }
	
	public void HeadTouchGround() { headTouchGround = true; }
	
	public void HandleTouchGround() { handleTouchGround = true; }
}
