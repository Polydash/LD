using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
	public float _Radius;
    private bool _active = false;

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

    public void activate()
    {
        _active = true;
        this.transform.GetChild(1).GetComponent<ParticleSystem>().enableEmission = true;
    }

    public void desactivate()
    {
        _active = false;
        this.transform.GetChild(1).GetComponent<ParticleSystem>().enableEmission = false;
    }

    public bool isActive()
    {
        return _active;
    }

}
