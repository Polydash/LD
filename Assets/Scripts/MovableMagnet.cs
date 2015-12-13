using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableMagnet : MonoBehaviour
{
	static List<MovableMagnet> _Instances;

	private float _Distance;

	public float _Speed;
	public float _StartTime;
	public Vector3 _Position1;
	public Vector3 _Position2;

	public static void RespawnAllInstances()
	{
		if(_Instances != null)
		{
			for(int i=0; i<_Instances.Count; ++i)
			{
				_Instances[i].Respawn();
			}
		}
	}

	private void Awake()
	{
		if(_Instances == null)
		{
			_Instances = new List<MovableMagnet>();
		}
		_Instances.Add(this);
	}

	private void OnDestroy()
	{
		_Instances.Remove(this);
	}

	private void Start()
	{
		_Distance = Vector3.Distance(_Position1, _Position2);
		_StartTime = Time.time;
	}

	private void Update()
	{
		float ratio = Mathf.Cos((Time.time - _StartTime) * _Speed / _Distance);
		ratio = (ratio + 1.0f) * 0.5f;
		transform.position = ratio * _Position1 + (1.0f - ratio) * _Position2;
	}

	private void Respawn()
	{
		_StartTime = Time.time;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(_Position1, 0.1f);
		Gizmos.DrawWireSphere(_Position2, 0.1f);
	}
}
