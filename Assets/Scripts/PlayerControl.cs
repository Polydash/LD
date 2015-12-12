using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	//Variables exposed to abilites
	public Rigidbody2D _Rigidbody {get; set;}
	public bool _GravityEnabled = true;
	public bool _MovementEnabled = true;
	public bool _IsOnGround {get; set;}
	public float _MoveInput {get; set;}

	private const float MOVEDEADZONE = 0.2f;
	private bool _JumpInput = false;

	//Vertical movement
	public float _GravityAcceleration;
	public float _GravityMaxSpeed;

	//Horizontal movement in air
	public float _AccelerationInAir;

	//Horizontal movement on ground
	public float _DeaccelerationOnGround;
	public float _AccelerationOnGround;
	public float _MaxSpeedOnGround;

	//Abilities
	Ability[] _Abilities;
	Ability _ActiveAbility = null;
	Ability _RequestedAbility = null;

	//Jump ability
	public float _JumpForce;
	public float _InAirJumpHorizontalForce;

	private void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody2D>();

		_Abilities = new Ability[1];
		_Abilities[0] = new JumpAbility();
	}
	
	private void Update()
	{
		_MoveInput = Input.GetAxis("Horizontal");
		if(Input.GetButtonDown("Fire1"))
		{
			_RequestedAbility = _Abilities[0];
		}
	}

	private void FixedUpdate()
	{
		//Jump
		if(_RequestedAbility != null && _ActiveAbility == null)
		{
			_ActiveAbility = _RequestedAbility;
			_ActiveAbility.StartAbility(this);
			_RequestedAbility = null;
		}

		if(_ActiveAbility != null && _ActiveAbility.IsActive())
		{
			_ActiveAbility.UpdateAbility();
		}
		else
		{
			_ActiveAbility = null;
		}

		//Horizontal Movement
		if(_MovementEnabled)
		{
			if(_IsOnGround)
			{
				OnGroundMovement();
			}
			else
			{
				InAirMovement();
			}
		}
		
		//Gravity
		if(_GravityEnabled)
		{
			_Rigidbody.velocity -= new Vector2(0.0f, _GravityAcceleration);
			if(_Rigidbody.velocity.y < -_GravityMaxSpeed)
			{
				float sign = (_Rigidbody.velocity.y > 0.0f) ? 1.0f : -1.0f;
				_Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, _GravityMaxSpeed * sign);
			}
		}
	}

	private void InAirMovement()
	{
		_Rigidbody.velocity += new Vector2(_MoveInput * _AccelerationInAir, 0.0f);
	}

	private void OnGroundMovement()
	{
		float inputSign = (_MoveInput > 0.0f) ? 1.0f : -1.0f;
		float velocitySign = (_Rigidbody.velocity.x > 0.0f) ? 1.0f : -1.0f;

		if(Mathf.Abs(_MoveInput) > MOVEDEADZONE && Mathf.Abs(_Rigidbody.velocity.x) <= _MaxSpeedOnGround)
		{
			_Rigidbody.velocity += new Vector2(_AccelerationOnGround * inputSign, 0.0f);
			if(Mathf.Abs(_Rigidbody.velocity.x) > _MaxSpeedOnGround)
			{
				_Rigidbody.velocity = new Vector2(_MaxSpeedOnGround * velocitySign, _Rigidbody.velocity.y);
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
