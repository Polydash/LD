using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
	public float _Radius;

	private void Start()
	{
		GetComponent<CircleCollider2D>().radius = _Radius;
		UnityEditor.SerializedObject so = new UnityEditor.SerializedObject(transform.GetChild(0).GetComponent<ParticleSystem>());
		so.FindProperty("ShapeModule.radius").floatValue = _Radius;
		so.ApplyModifiedProperties();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _Radius);
	}
}
