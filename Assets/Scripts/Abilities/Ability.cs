using UnityEngine;
using System.Collections;

public abstract class Ability
{
	protected PlayerControl _Player;

	protected bool _IsActive = false;
	protected bool _IsAvailable = true;

	public void StartAbility(PlayerControl playerControl)
	{
		_Player = playerControl;
		_IsActive = true;
		OnStart();
		_Player._IsOnGround = false;
	}

	public void UpdateAbility()
	{
		OnUpdate();
	}

	public void EndAbility()
	{
		OnEnd();
		_IsActive = false;
	}

	public bool IsActive()
	{
		return _IsActive;
	}

	protected abstract void OnStart();
	protected abstract void OnUpdate();
	protected abstract void OnEnd();
}
