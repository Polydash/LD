using UnityEngine;
using System.Collections;

public class JumpAbility : Ability
{
	protected override void OnStart()
	{
		_Player._Rigidbody.velocity = new Vector2(_Player._Rigidbody.velocity.x, _Player._JumpForce);
		if(!_Player._IsOnGround)
		{
			_Player._Rigidbody.velocity += new Vector2(_Player._MoveInput * _Player._InAirJumpHorizontalForce, 0.0f);
		}
		
		EndAbility();
	}

	protected override void OnUpdate()
	{
	}

	protected override void OnEnd()
	{
	}
}
