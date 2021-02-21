using System;
using UnityEngine;

using Vec2f = UnityEngine.Vector2;
using Vec3f = UnityEngine.Vector3;
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
	private float jumpCastWidth = 1.0f;
	[SerializeField]
	private Transform jumpRayOrigin;
	[SerializeField]
	private LayerMask groundLayers;
	[SerializeField]
	private LayerMask blockLayer;
	private int state = (int) PlayerState.CAN_MOVE;

	[Header("Block Moving")]
	[SerializeField]
	private float grabMaxRange = 1.0f;
	[SerializeField]
	private float blockMoveSpeed = 1.0f;
	private PushableBlock heldBlock;

	[Header("Components")]
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private new Transform transform;
	[SerializeField]
	private new Rigidbody2D rigidbody;
	[SerializeField]
	private new BoxCollider2D collider;

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

			if ((state & (int) PlayerState.PUSHING_BLOCK) == 0) CheckJump();
		}
		else
		{
			rigidbody.velocity = Vec2f.zero;
			if (heldBlock != null)
			{
				transform.localScale =
					new Vec2f(Math.Sign(heldBlock.Transform.position.x - transform.position.x),
						transform.localScale.y);
			}
		}

		PushBlock();
	}

	private void CheckMove()
	{
		float gravityY = Math.Sign(Physics2D.gravity.y);
		float moveX    = Input.GetAxis("Horizontal");
		var newRight   = (Quaternion.Euler(0.0f, 0.0f, 90.0f) * Physics2D.gravity).normalized;
		var velocity   = rigidbody.velocity;

		if (Physics2D.gravity.y != 0.0f)
		{
			velocity = (state & (int) PlayerState.PUSHING_BLOCK) != 0 ?
                           new Vec3f(moveX * blockMoveSpeed * -gravityY, velocity.y, 0.0f) :
                           new Vec3f(moveX * moveSpeed * -gravityY, velocity.y, 0.0f);
			
			animator.SetFloat(MoveX, Math.Abs(velocity.x));
			animator.SetFloat(PushMoveX, velocity.x * transform.localScale.x);
		}
		else if (Physics2D.gravity.x != 0.0f)
		{
			velocity = (state & (int) PlayerState.PUSHING_BLOCK) != 0 ?
                           new Vec3f(velocity.x, moveX * blockMoveSpeed * newRight.y, 0.0f) :
                           new Vec3f(velocity.x, moveX * moveSpeed * newRight.y, 0.0f);
			
			animator.SetFloat(MoveX, Math.Abs(velocity.y));
			animator.SetFloat(PushMoveX, velocity.y * transform.localScale.x);
		}

		rigidbody.velocity = velocity;

		if (Physics2D.gravity.y != 0.0f)
		{
			if (Math.Abs(rigidbody.velocity.x) < 0.1f)
				rigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
			else
				rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
		}
		else if (Physics2D.gravity.x != 0.0f)
		{
			if (Math.Abs(rigidbody.velocity.y) < 0.1f)
				rigidbody.constraints |= RigidbodyConstraints2D.FreezePositionY;
			else
				rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
		}

		if (!Input.GetButton("Interact"))
		{
			var sign = 0;
			if (Physics2D.gravity.y != 0.0f)
				sign = Math.Sign((transform.localRotation * velocity).x);
			else if (Physics2D.gravity.x != 0.0f)
				sign = Math.Sign(-(transform.localRotation * velocity).x);
			if (sign == 0) return;

			transform.localScale = new Vec3f(sign, 1.0f, 1.0f);
		}
	}

	private void CheckJump()
	{
		float gravityX = Mathf.Sign(Physics2D.gravity.x);
		float gravityY = Math.Sign(Physics2D.gravity.y);
		var newUp      = -new Vec2f(gravityX, gravityY);
		var hit        = Physics2D.CircleCast(
            jumpRayOrigin.position, jumpCastWidth, -newUp, jumpGroundDist, groundLayers);

		if (hit) state |= (int) PlayerState.CAN_JUMP;
		else state &= (int) ~PlayerState.CAN_JUMP;
		
		animator.SetBool(CanJump, (state & (int) PlayerState.CAN_JUMP) != 0);

		if (Input.GetButtonDown("Jump") && (state & (int) PlayerState.CAN_JUMP) != 0)
		{
			animator.SetTrigger(Jump);
			rigidbody.velocity = newUp * jumpStrength;
		}
		else if (!Input.GetButton("Jump"))
		{
			if (Physics2D.gravity.y != 0.0f && rigidbody.velocity.y * -gravityY > 0.0f)
			{
				rigidbody.velocity +=
					newUp * (Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime * -gravityY);
			}
			else if (Physics2D.gravity.x != 0.0f && rigidbody.velocity.x * -gravityX > 0.0f)
			{
				rigidbody.velocity +=
					newUp * (Physics2D.gravity.x * lowJumpMultiplier * Time.deltaTime * -gravityX);
			}
		}
		else
		{
			animator.ResetTrigger(Jump);
		}

		if (Physics2D.gravity.y != 0.0f)
		{
			animator.SetFloat(VelocityY, rigidbody.velocity.y * -gravityY);
			rigidbody.gravityScale =
				rigidbody.velocity.y * -gravityY < -2.0f ? fallMultiplier : 1.0f;
		}
		else if (Physics2D.gravity.x != 0.0f)
		{
			animator.SetFloat(VelocityY, rigidbody.velocity.x * -gravityX);
			rigidbody.gravityScale =
				rigidbody.velocity.x * -gravityX < -2.0f ? fallMultiplier : 1.0f;
		}
	}

	private void PushBlock()
	{
		if (rigidbody.velocity.y != 0.0f) return;

		animator.SetBool(PushingBlock, (state & (int) PlayerState.PUSHING_BLOCK) != 0);
		var dirRay = Vec3f.right * (transform.localScale.x * 1.0f);
		var hit    = Physics2D.Raycast(transform.position, dirRay, grabMaxRange, blockLayer);
		if (hit)
		{
			if (Input.GetButtonDown("Interact"))
			{
				Debug.Log("Grab");

				heldBlock = new PushableBlock(hit.collider.gameObject);

				var blockSize  = heldBlock.Collider.size;
				var playerSize = GetComponent<BoxCollider2D>().size;
				if (heldBlock.Transform.position.x - transform.position.x > 0)
				{
					transform.position = new Vec2f(
						heldBlock.Transform.position.x - blockSize.x / 2.0f - playerSize.x / 2.0f,
						transform.position.y);
				}
				else
				{
					transform.position = new Vec2f(
						heldBlock.Transform.position.x + blockSize.x / 2.0f + playerSize.x / 2.0f,
						transform.position.y);
				}
				rigidbody.velocity = Vec2f.zero;

				state = 0;
				return;
			}

			if (hit && Input.GetButton("Interact"))
			{
				Debug.Log("Grabbing");

				heldBlock.Rigidbody.mass      = 1.0f;
				heldBlock.Joint.enabled       = true;
				heldBlock.Joint.connectedBody = rigidbody;

				state = (int) PlayerState.PUSHING_BLOCK | (int) PlayerState.CAN_MOVE;
				return;
			}
		}

		if (heldBlock != null)
		{
			Debug.Log("Ungrab");

			rigidbody.velocity = Vec2f.zero;

			heldBlock.Rigidbody.mass     = 1000.0f;
			heldBlock.Rigidbody.velocity = Vec2f.zero;
			heldBlock.Joint.enabled      = false;

			heldBlock = null;
			state     = (int) PlayerState.CAN_MOVE;
		}
	}

	private void OnDrawGizmos()
	{
		var dirRay   = Vec3f.right * (transform.localScale.x * grabMaxRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + dirRay);

		float gravityX = Math.Sign(Physics2D.gravity.x);
		float gravityY = Math.Sign(Physics2D.gravity.y);
		var newDown    = new Vec2f(gravityX, gravityY);
		dirRay         = newDown * jumpGroundDist;
		Gizmos.DrawCube(
			jumpRayOrigin.position + dirRay / 2.0f, new Vec3f(jumpCastWidth, jumpGroundDist));
	}
}