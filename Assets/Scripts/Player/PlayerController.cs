using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Player Movements")]
	[SerializeField] private float moveSpeed = 5.0f;
	[SerializeField] private float jumpStrength = 7.5f;
	[SerializeField] private float fallMultiplier = 1.5f;
	[SerializeField] private float lowJumpMultiplier = 0.1f;
	[SerializeField] private Transform jumpRaycastOrigin;
	private bool canJump;

	[Header("Components")]
	[SerializeField] private Animator animator;
	private Transform aTransform;
	private Rigidbody2D aRigidbody;
	
	[Header("Animator")]
	private static readonly int Jump = Animator.StringToHash("Jump");
	private static readonly int VelocityY = Animator.StringToHash("VelocityY");
	private static readonly int CanJump = Animator.StringToHash("CanJump");
	private static readonly int MoveX = Animator.StringToHash("MoveX");

	// Start is called before the first frame update
	private void Start()
	{
		aTransform = transform;
		aRigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		CheckMove();
		CheckJump();
	}

	private void CheckMove()
	{
		float moveX = Input.GetAxis("Horizontal");
		animator.SetFloat(MoveX, Math.Abs(moveX));
		aRigidbody.velocity = new Vector3(moveX * moveSpeed, aRigidbody.velocity.y, 0.0f);

		if (moveX < 0.0f)
			aTransform.localScale = new Vector3(-1.0f, 1.0f);
		else if (moveX > 0.0f)
			aTransform.localScale = new Vector3(1.0f, 1.0f);
	}

	private void CheckJump()
	{
		int groundLayer = 1 << LayerMask.NameToLayer("Ground");
		var hit =
			Physics2D.CircleCast(jumpRaycastOrigin.position, 0.1f, Vector2.down, 0.2f, groundLayer);
		canJump = hit;
		animator.SetBool(CanJump, canJump);
		
		if (Input.GetButtonDown("Jump") && canJump)
		{
			animator.SetTrigger(Jump);
			aRigidbody.velocity = Vector2.up * jumpStrength;
		}
		else if (aRigidbody.velocity.y > 0.0f && !Input.GetButton("Jump"))
		{
			aRigidbody.velocity += 
				Vector2.up * (Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime);
		}
		else
		{
			animator.ResetTrigger(Jump);
		}

		animator.SetFloat(VelocityY, aRigidbody.velocity.y);
		aRigidbody.gravityScale = aRigidbody.velocity.y < 0.0f ? fallMultiplier : 1.0f;
	}

	private void PushBlock()
	{
		if (Input.GetButton("Interact"))
		{
			
		}
	}
}
