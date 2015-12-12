﻿using UnityEngine;
using System.Collections;

public class LerpCamera : MonoBehaviour
{
	public Transform _Player;
	public float _Lerp;

	public bool _Constrained;
	public float _MinX;
	public float _MaxX;
	public float _MinY;
	public float _MaxY;

	private void Update()
	{
		transform.position += (_Player.position - transform.position) * _Lerp;
 		transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);

		if(_Constrained)
		{
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, _MinX, _MaxX),
											Mathf.Clamp(transform.position.y, _MinY, _MaxY),
											transform.position.z);
		}
	}
}
