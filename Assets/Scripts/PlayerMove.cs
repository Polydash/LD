using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
	private Rigidbody2D _Rigidbody;
	private Animator _Animator;
	private float _Input;
	private List<GameObject> _MagnetList;
	private GameObject _MasterMagnet = null;
	private bool _AttractionRequested = false;
	private bool _RepulsionRequested = false;
	private bool _IsOnGround = false;

	public float _MovementForce;

	public float _MasterAttractionMinForce;
	public float _MasterAttractionForce;
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

	public float _ToupieAnimationThreshold;
	public float _ToupieAnimationMaxSpeed;

	private IEnumerator FreezeFrame(float time)
	{
		Time.timeScale = 0.01f;
		yield return new WaitForSeconds(time);
		Time.timeScale = 1;
	}

	private void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody2D>();
		_MagnetList = new List<GameObject>();
		_Animator = transform.GetChild(0).GetComponent<Animator>();
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

		//Master magnet
		if(_MasterMagnet != null)
		{
			AttractMasterMagnet();
		}

		//Attraction
		if(_AttractionRequested)
		{
			Attract();
			_AttractionRequested = false;
		}

		//Repulsion
		if(_RepulsionRequested)
		{
			Repulse();
			_RepulsionRequested = false;
		}

		//Animation state
		_Animator.speed = 1.0f;
		if(Mathf.Abs(_Rigidbody.velocity.y) > 0.5f || !_IsOnGround)
		{
			_Animator.SetBool("toupie", true);
			transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
			float magnitude = _Rigidbody.velocity.magnitude;
			if(magnitude > _ToupieAnimationThreshold)
			{
				float animationSpeed = Mathf.Min(magnitude / _ToupieAnimationThreshold, _ToupieAnimationMaxSpeed);
				_Animator.speed = animationSpeed;
			}
			_IsOnGround = false;
		}
		else
		{
			_Animator.SetBool("toupie", false);
			float speed = Mathf.Abs(_Rigidbody.velocity.x) / 4.5f * 100.0f;
			_Animator.SetFloat("Speed", speed);

			if(_Rigidbody.velocity.x < -0.5f)
			{
				transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0.0f, 230.0f, 0.0f));
			}
			else if(_Rigidbody.velocity.x > 0.5f)
			{
				transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0.0f, 130.0f, 0.0f));
			}
		}
	}

	private void AttractMasterMagnet()
	{
		Vector3 attractForce = Vector3.zero;
		Vector3 distance = _MasterMagnet.transform.position - transform.position;
		float magnitude = distance.magnitude;
		distance.Normalize();

		float forceRatio = Mathf.Max(0.0f, 1.0f - (magnitude / _MasterMagnet.GetComponent<CircleCollider2D>().radius));
		float forceMode = _MasterAttractionForce;
		forceMode = _MasterAttractionMinForce + (forceMode - _MasterAttractionMinForce) * forceRatio;
		attractForce += distance * forceMode;

		_Rigidbody.AddForce(attractForce, ForceMode2D.Impulse);
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

		if(_MagnetList.Count > 0)
		{
			Camera.main.transform.position -= repulsionForce * 0.0075f;
			StartCoroutine(FreezeFrame(0.0005f));
		}

		_Rigidbody.AddForce(repulsionForce, ForceMode2D.Impulse);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Magnet" && _MasterMagnet == null)
		{
			_MagnetList.Add(collider.gameObject);
		}
		else if(collider.tag == "MasterMagnet")
		{
			_MagnetList.Clear();
			_MasterMagnet = collider.gameObject;
			Camera.main.GetComponent<LerpCamera>()._ShakeValue = 0.1f;
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag == "Magnet")
		{
			_MagnetList.Remove(collider.gameObject);
		}
		else if(collider.tag == "MasterMagnet")
		{
			_MasterMagnet = null;
			Camera.main.GetComponent<LerpCamera>()._ShakeValue = 0.0f;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.contacts.Length > 0)
		{
			if(collision.contacts[0].normal.y > 0.0f && _Rigidbody.velocity.y <= 0.0f)
			{
				_IsOnGround = true;
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.contacts.Length > 0)
		{
			if(collision.contacts[0].normal.y > 0.0f && _Rigidbody.velocity.y <= 0.0f)
			{
				_IsOnGround = true;
			}
		}
	}
}
