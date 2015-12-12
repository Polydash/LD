using UnityEngine;
using System.Collections;

public class MovableMagnet : MonoBehaviour
{
	private float _Distance;

	public float _Speed;
	public Vector3 _Position1;
	public Vector3 _Position2;

	private void Start()
	{
		_Distance = Vector3.Distance(_Position1, _Position2);
	}

	private void Update()
	{
		float ratio = Mathf.Cos(Time.time * _Speed / _Distance);
		ratio = (ratio + 1.0f) * 0.5f;
		transform.position = ratio * _Position1 + (1.0f - ratio) * _Position2;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(_Position1, 0.1f);
		Gizmos.DrawWireSphere(_Position2, 0.1f);
	}
}
