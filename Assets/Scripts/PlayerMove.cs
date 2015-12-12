using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
	private Rigidbody2D _Rigidbody;
	private float _Input;
	private List<GameObject> _MagnetList;
	private bool _AttractionRequested = false;
	private bool _RepulsionRequested = false;

	public float _MovementForce;

	public float _AttractionImpulseForce;
	public float _AttractionMinForce;
	public float _AttractionForce;
	public float _AttractionForceCap;
	public bool _AttractionImpulse = false;
	public float _RepulsionImpulseForce;
	public float _RepulsionMinForce;
	public float _RepulsionForce;
	public float _RepulsionForceCap;
	public bool _RepulsionImpulse = false;

	private void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody2D>();
		_MagnetList = new List<GameObject>();
	}

	private void Update()
	{
		_Input = Input.GetAxis("Horizontal");

		if(Input.GetButton("Fire1") && !_AttractionImpulse)
		{
			_AttractionRequested = true;
		}

		if(Input.GetButton("Fire2") && !_RepulsionImpulse)
		{
			_RepulsionRequested = true;
		}

		if(Input.GetButtonDown("Fire1") && _AttractionImpulse)
		{
			_AttractionRequested = true;
		}

		if(Input.GetButtonDown("Fire2") && _RepulsionImpulse)
		{
			_RepulsionRequested = true;
		}
	}

	private void FixedUpdate()
	{
		_Rigidbody.AddForce(new Vector2(_Input * _MovementForce, 0.0f), ForceMode2D.Force);

		if(_AttractionRequested)
		{
			Attract();
			_AttractionRequested = false;
		}

		if(_RepulsionRequested)
		{
			Repulse();
			_RepulsionRequested = false;
		}
	}

	private void Attract()
	{
		Vector3 attractForce = Vector3.zero;
		for(int i=0; i<_MagnetList.Count; ++i)
		{
			Vector3 distance = _MagnetList[i].transform.position - transform.position;
			float magnitude = distance.magnitude;
			distance.Normalize();

			float forceRatio = Mathf.Max(0.0f, 1.0f - (magnitude / _MagnetList[i].GetComponent<CircleCollider2D>().radius));
			float forceMode = (_AttractionImpulse) ? _AttractionImpulseForce : Mathf.Max(_AttractionForce, _AttractionForceCap);
			forceMode = _AttractionMinForce + (forceMode - _AttractionMinForce) * forceRatio;
			attractForce += distance * forceMode;
		}

		_Rigidbody.AddForce(attractForce, ForceMode2D.Impulse);
	}

	private void Repulse()
	{
		Vector3 repulsionForce = Vector3.zero;
		for(int i=0; i<_MagnetList.Count; ++i)
		{
			Vector3 distance = _MagnetList[i].transform.position - transform.position;
			float magnitude = distance.magnitude;
			distance.Normalize();

			float forceRatio = Mathf.Max(0.0f, 1.0f - (magnitude / _MagnetList[i].GetComponent<CircleCollider2D>().radius));
			float forceMode = (_RepulsionImpulse) ? _RepulsionImpulseForce : Mathf.Max(_RepulsionForce, _RepulsionForceCap);
			forceMode = _RepulsionMinForce + (forceMode - _RepulsionMinForce) * forceRatio;
			repulsionForce -= distance * forceMode;	
		}
		
		_Rigidbody.AddForce(repulsionForce, ForceMode2D.Impulse);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Magnet")
		{
			_MagnetList.Add(collider.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag == "Magnet")
		{
			_MagnetList.Remove(collider.gameObject);
		}
	}
}
