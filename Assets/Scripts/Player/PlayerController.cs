using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Player Movements")]
	[SerializeField]
	private float moveSpeed = 5.0f;
	[SerializeField]
	private float jumpStrength = 7.5f;
	[SerializeField]
	private float fallMultiplier = 1.5f;
	[SerializeField]
	private float lowJumpMultiplier = 0.1f;
	[SerializeField]
	private float jumpGroundDist = 0.2f;
	[SerializeField]
	private Transform jumpRaycastOrigin;
	[SerializeField]
	private LayerMask groundLayer;
	private int state = (int) PlayerState.CAN_MOVE;

	[Header("Block Moving")]
	[SerializeField]
	private float grabMaxRange = 1.0f;
	[SerializeField]
	private float blockMoveSpeed = 1.0f;
	private GameObject heldBlock;
	private BoxCollider2D heldBlockCollider;
	private Rigidbody2D heldBlockRb;
	private FixedJoint2D heldBlockJoint;

	[Header("Components")]
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private Transform aTransform;
	[SerializeField]
	private Rigidbody2D aRigidbody;

	[Header("Animator")]
	private static readonly int Jump         = Animator.StringToHash("Jump");
	private static readonly int VelocityY    = Animator.StringToHash("VelocityY");
	private static readonly int CanJump      = Animator.StringToHash("CanJump");
	private static readonly int MoveX        = Animator.StringToHash("MoveX");
	private static readonly int PushingBlock = Animator.StringToHash("PushingBlock");
	private static readonly int PushMoveX    = Animator.StringToHash("PushMoveX");

	[Flags]
	public enum PlayerState { NONE = 0, CAN_MOVE = 1 << 0, CAN_JUMP = 1 << 1, PUSHING_BLOCK = 1 << 2 }

	public void SetState(int newState) { state = newState; }

	private void Update()
	{
		if ((state & (int) PlayerState.CAN_MOVE) != 0)
		{
			CheckMove();
			CheckJump();
		}
		else
		{
			aRigidbody.velocity = Vector2.zero;
			if ((state & (int) PlayerState.PUSHING_BLOCK) != 0)
			{
				aTransform.localScale =
					new Vector2(Math.Sign(heldBlock.transform.position.x - aTransform.position.x),
						aTransform.localScale.y);
			}
		}

		PushBlock();
	}

	private void CheckMove()
	{
		float moveX  = Input.GetAxis("Horizontal");
		var velocity = aRigidbody.velocity;

		velocity = (state & (int) PlayerState.PUSHING_BLOCK) != 0 ?
                       new Vector3(moveX * blockMoveSpeed, velocity.y, 0.0f) :
                       new Vector3(moveX * moveSpeed, velocity.y, 0.0f);

		aRigidbody.velocity = velocity;
		animator.SetFloat(MoveX, Math.Abs(velocity.x));
		animator.SetFloat(PushMoveX, velocity.x * aTransform.localScale.x);

		if (Math.Abs(aRigidbody.velocity.x) < 0.1f)
			aRigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
		else
			aRigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

		if (!Input.GetButton("Interact"))
		{
			if (moveX < 0.0f) aTransform.localScale = new Vector3(-1.0f, 1.0f);
			else if (moveX > 0.0f) aTransform.localScale = new Vector3(1.0f, 1.0f);
		}
	}

	private void CheckJump()
	{
		var hit = Physics2D.CircleCast(
			jumpRaycastOrigin.position, 0.2f, Vector2.down, jumpGroundDist, groundLayer);

		if (hit) state |= (int) PlayerState.CAN_JUMP;
		else
			state &= (int) ~PlayerState.CAN_JUMP;
		animator.SetBool(CanJump, (state & (int) PlayerState.CAN_JUMP) != 0);

		if (Input.GetButtonDown("Jump") && (state & (int) PlayerState.CAN_JUMP) != 0)
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
		aRigidbody.gravityScale = aRigidbody.velocity.y < -2.0f ? fallMultiplier : 1.0f;
	}

	private void PushBlock()
	{
		if ((state & (int) PlayerState.CAN_JUMP) == 0) return;

		animator.SetBool(PushingBlock, (state & (int) PlayerState.PUSHING_BLOCK) != 0);
		var dirRay = Vector3.right * (aTransform.localScale.x * 1.0f);
		var hit    = Physics2D.Raycast(aTransform.position, dirRay, grabMaxRange, groundLayer);
		if (hit && hit.collider.CompareTag("PushableBloc") && Input.GetButton("Interact"))
		{
			if (Input.GetButtonDown("Interact"))
			{
				heldBlock         = hit.collider.gameObject;
				heldBlockRb       = heldBlock.GetComponent<Rigidbody2D>();
				heldBlockCollider = heldBlock.GetComponent<BoxCollider2D>();
				heldBlockJoint    = heldBlock.GetComponent<FixedJoint2D>();

				if (heldBlock.transform.position.x - aTransform.position.x > 0)
				{
					var blockSize       = heldBlockCollider.size;
					var playerSize      = GetComponent<BoxCollider2D>().size;
					aTransform.position = new Vector2(
						heldBlock.transform.position.x - blockSize.x / 2.0f - playerSize.x / 2.0f,
						aTransform.position.y);
					aRigidbody.velocity = Vector2.zero;
				}
				else
				{
					var blockSize       = heldBlockCollider.size;
					var playerSize      = GetComponent<BoxCollider2D>().size;
					aTransform.position = new Vector2(
						heldBlock.transform.position.x + blockSize.x / 2.0f + playerSize.x / 2.0f,
						aTransform.position.y);
					aRigidbody.velocity = Vector2.zero;
				}
			}

			heldBlockRb.mass             = 1.0f;
			heldBlockJoint.enabled       = true;
			heldBlockJoint.connectedBody = aRigidbody;

			state = (int) PlayerState.PUSHING_BLOCK | (int) PlayerState.CAN_MOVE;
		}
		else if (Input.GetButtonUp("Interact") && heldBlock)
		{
			aRigidbody.velocity = Vector2.zero;

			heldBlockRb.mass       = 1000.0f;
			heldBlockRb.velocity   = Vector2.zero;
			heldBlockJoint.enabled = false;
			
			state = (int) PlayerState.CAN_MOVE;
		}
	}

	private void OnDrawGizmos()
	{
		var dirRay   = Vector3.right * (aTransform.localScale.x * grabMaxRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(aTransform.position, aTransform.position + dirRay);

		dirRay = Vector3.down * jumpGroundDist;
		Gizmos.DrawLine(jumpRaycastOrigin.position, jumpRaycastOrigin.position + dirRay);
	}
}