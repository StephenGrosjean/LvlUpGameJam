using System;
using UnityEngine;

using Vec2f = UnityEngine.Vector2;
using Vec3f = UnityEngine.Vector3;
public class PlayerController : MonoBehaviour
{
	[Header("Player Movements")]
	[SerializeField] private float moveSpeed = 5.0f;
	[SerializeField] private float jumpStrength = 7.5f;
	[SerializeField] private float fallMultiplier = 1.5f;
	[SerializeField] private float lowJumpMultiplier = 0.1f;
	[SerializeField] private float jumpGroundDist = 0.2f;
	[SerializeField] private float jumpCastWidth = 1.0f;
	[SerializeField] private Transform jumpRayOrigin;
	[SerializeField] private LayerMask groundLayer;
	private int state = (int) PlayerState.CAN_MOVE;

	[Header("Block Moving")]
	[SerializeField] private float grabMaxRange = 1.0f;
	[SerializeField] private float blockMoveSpeed = 1.0f;
	private GameObject heldBlock;
	private BoxCollider2D heldBlockCollider;
	private Rigidbody2D heldBlockRb;
	private FixedJoint2D heldBlockJoint;

	[Header("Components")]
	[SerializeField] private Animator animator;
	[SerializeField] private new Transform transform;
	[SerializeField] private new Rigidbody2D rigidbody;
	[SerializeField] private new BoxCollider2D collider;

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
			rigidbody.velocity = Vec2f.zero;
			if ((state & (int) PlayerState.PUSHING_BLOCK) != 0)
			{
				transform.localScale =
					new Vec2f(Math.Sign(heldBlock.transform.position.x - transform.position.x),
						transform.localScale.y);
			}
		}

		PushBlock();
	}

	private void CheckMove()
	{
		float gravityY = Math.Sign(Physics2D.gravity.y);
		float moveX  = Input.GetAxis("Horizontal");
		var velocity = rigidbody.velocity;

		velocity = (state & (int) PlayerState.PUSHING_BLOCK) != 0 ?
                       new Vec3f(moveX * blockMoveSpeed * -gravityY, velocity.y, 0.0f) :
                       new Vec3f(moveX * moveSpeed * -gravityY, velocity.y, 0.0f);

		rigidbody.velocity = velocity;
		animator.SetFloat(MoveX, Math.Abs(velocity.x));
		animator.SetFloat(PushMoveX, velocity.x * transform.localScale.x);

		if (Math.Abs(rigidbody.velocity.x) < 0.1f)
			rigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
		else
			rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

		if (!Input.GetButton("Interact"))
		{
			Debug.Log(transform.localRotation * velocity);
			int sign = Math.Sign((transform.localRotation * velocity).x);
			transform.localScale = new Vec3f(sign == 0 ? 1 : sign, 1.0f, 1.0f);
		}
	}

	private void CheckJump()
	{
		float gravityY = Math.Sign(Physics2D.gravity.y);
		var newUp      = new Vec2f(0.0f, -gravityY);
		var hit        = Physics2D.CircleCast(
            jumpRayOrigin.position, jumpCastWidth, -newUp, jumpGroundDist, groundLayer);

		if (hit) state |= (int) PlayerState.CAN_JUMP;
		else
			state &= (int) ~PlayerState.CAN_JUMP;
		animator.SetBool(CanJump, (state & (int) PlayerState.CAN_JUMP) != 0);

		if (Input.GetButtonDown("Jump") && (state & (int) PlayerState.CAN_JUMP) != 0)
		{
			animator.SetTrigger(Jump);
			rigidbody.velocity = newUp * jumpStrength;
		}
		else if (rigidbody.velocity.y * -gravityY > 0.0f && !Input.GetButton("Jump"))
		{
			rigidbody.velocity +=
				newUp * (Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime * -gravityY);
		}
		else
		{
			animator.ResetTrigger(Jump);
		}

		animator.SetFloat(VelocityY, rigidbody.velocity.y * -gravityY);
		rigidbody.gravityScale = rigidbody.velocity.y * -gravityY < -2.0f ? fallMultiplier : 1.0f;
	}

	private void PushBlock()
	{
		if ((state & (int) PlayerState.CAN_JUMP) == 0) return;

		animator.SetBool(PushingBlock, (state & (int) PlayerState.PUSHING_BLOCK) != 0);
		var dirRay = Vec3f.right * (transform.localScale.x * 1.0f);
		var hit    = Physics2D.Raycast(transform.position, dirRay, grabMaxRange, groundLayer);
		if (hit && hit.collider.CompareTag("PushableBloc") && Input.GetButton("Interact"))
		{
			if (Input.GetButtonDown("Interact"))
			{
				heldBlock         = hit.collider.gameObject;
				heldBlockRb       = heldBlock.GetComponent<Rigidbody2D>();
				heldBlockCollider = heldBlock.GetComponent<BoxCollider2D>();
				heldBlockJoint    = heldBlock.GetComponent<FixedJoint2D>();

				if (heldBlock.transform.position.x - transform.position.x > 0)
				{
					var blockSize       = heldBlockCollider.size;
					var playerSize      = GetComponent<BoxCollider2D>().size;
					transform.position = new Vec2f(
						heldBlock.transform.position.x - blockSize.x / 2.0f - playerSize.x / 2.0f,
						transform.position.y);
					rigidbody.velocity = Vec2f.zero;
				}
				else
				{
					var blockSize       = heldBlockCollider.size;
					var playerSize      = GetComponent<BoxCollider2D>().size;
					transform.position = new Vec2f(
						heldBlock.transform.position.x + blockSize.x / 2.0f + playerSize.x / 2.0f,
						transform.position.y);
					rigidbody.velocity = Vec2f.zero;
				}
			}

			heldBlockRb.mass             = 1.0f;
			heldBlockJoint.enabled       = true;
			heldBlockJoint.connectedBody = rigidbody;

			state = (int) PlayerState.PUSHING_BLOCK | (int) PlayerState.CAN_MOVE;
		}
		else if (Input.GetButtonUp("Interact") && heldBlock)
		{
			rigidbody.velocity = Vec2f.zero;

			heldBlockRb.mass       = 1000.0f;
			heldBlockRb.velocity   = Vec2f.zero;
			heldBlockJoint.enabled = false;
			
			state = (int) PlayerState.CAN_MOVE;
		}
	}

	private void OnDrawGizmos()
	{
		var dirRay   = Vec3f.right * (transform.localScale.x * grabMaxRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + dirRay);

		float gravityY = Math.Sign(Physics2D.gravity.y);
		var newDown = new Vec2f(0.0f, gravityY);
		dirRay      = newDown * jumpGroundDist;
		Gizmos.DrawCube(
			jumpRayOrigin.position + dirRay / 2.0f, new Vec3f(jumpCastWidth, jumpGroundDist));
	}
}