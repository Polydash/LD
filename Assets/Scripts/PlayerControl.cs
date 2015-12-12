using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	private Rigidbody2D _Rigidbody;
	
	private const float MOVEDEADZONE = 0.2f;
	private bool _JumpInput = false;
	private float _MoveInput;

	private bool _IsOnGround = false;

	//Jump
	public float _JumpForce;

	//Vertical movement
	public float _GravityAcceleration;
	public float _GravityMaxSpeed;

	//Horizontal movement on ground
	public float _DeaccelerationOnGround;
	public float _AccelerationOnGround;
	public float _MaxSpeedOnGround;

	private void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody2D>();
	}
	
	private void Update()
	{
		_MoveInput = Input.GetAxis("Horizontal");
		if(Input.GetButtonDown("Jump"))
		{
			_JumpInput = true;
		}
	}

	private void FixedUpdate()
	{
		//Jump
		if(_JumpInput)
		{
			if(_IsOnGround)
			{
				Jump();
			}
			_JumpInput = false;
		}

		//Horizontal Movement
		if(_IsOnGround)
		{
			OnGroundMovement();
		}
		
		//Gravity
		_Rigidbody.velocity -= new Vector2(0.0f, _GravityAcceleration);
		if(_Rigidbody.velocity.y < -_GravityMaxSpeed)
		{
			float sign = (_Rigidbody.velocity.y > 0.0f) ? 1.0f : -1.0f;
			_Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, _GravityMaxSpeed * sign);
		}
	}

	private void OnGroundMovement()
	{
		float inputSign = (_MoveInput > 0.0f) ? 1.0f : -1.0f;
		float velocitySign = (_Rigidbody.velocity.x > 0.0f) ? 1.0f : -1.0f;

		//If input, accelerate and clamp max speed
		if(Mathf.Abs(_MoveInput) > MOVEDEADZONE)
		{
			//Acceleration
			if(Mathf.Abs(_Rigidbody.velocity.x) <= _MaxSpeedOnGround)
			{
				_Rigidbody.velocity += new Vector2(_AccelerationOnGround * inputSign, 0.0f);
				if(Mathf.Abs(_Rigidbody.velocity.x) > _MaxSpeedOnGround)
				{
					_Rigidbody.velocity = new Vector2(_MaxSpeedOnGround * velocitySign, _Rigidbody.velocity.y);
				}
			}
		}
		else
		{
			//Deacceleration (friction)
			if(Mathf.Abs(_Rigidbody.velocity.x) <= _DeaccelerationOnGround)
			{
				_Rigidbody.velocity = new Vector2(0.0f, _Rigidbody.velocity.y);
			}
			else
			{
				_Rigidbody.velocity += new Vector2(_DeaccelerationOnGround * velocitySign * -1.0f, 0.0f); 
			}
		}
	}

	private void Jump()
	{
		_Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, _JumpForce);
		_IsOnGround = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.contacts.Length > 0)
		{
			float upDirection = Vector2.Dot(collision.contacts[0].normal, Vector2.up);
			if(upDirection > 0.5f)
			{
				_IsOnGround = true;
				_Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, 0.0f);
			}
		}
	}
}
