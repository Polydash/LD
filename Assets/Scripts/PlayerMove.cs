using UnityEngine;
using UnityEngine.UI;
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
	private float _SuckedRotation;
	private bool _IsDead = false;
	private float _DeathAnimationElapsed = 0.0f;
	private Vector3 _MeshOffset;
	private Vector3 _StartPosition;
	public float _LevelStartTime {get; set;}
	public float _LevelEndTime {get; set;}
	private bool _LevelEnded {get; set;}

	[Header("Ground Movement")]
	public float _MovementForce;

	[Header("Master Magnet")]
	public AnimationCurve _MasterAttractionCurve;
	public float _MasterAttractionMinForce;
	public float _MasterAttractionForce;

	[Header("Common Magnet Attraction")]
	public bool _WithCurve = false;
	public AnimationCurve _AttractionCurve;
	public float _AttractionImpulseForce;
	public float _AttractionMinForce;
	public float _AttractionForce;
	public float _AttractionForceCap;
	public bool _AttractionImpulse = false;
	
	[Header("Common Magnet Repulsion")]
	public float _RepulsionImpulseForce;
	public float _RepulsionMinForce;
	public float _RepulsionForce;
	public float _RepulsionForceCap;
	public bool _RepulsionImpulse = false;

	[Header("Animation")]
	public float _ToupieAnimationThreshold;
	public float _ToupieAnimationMaxSpeed;
	public float _SuckedSpeed;
	public float _DeathAnimationTime;
	public float _MaxDeathShake;
	public float _WarpAnimationTime;

	private IEnumerator FreezeFrame(float time)
	{
		Time.timeScale = 0.01f;
		yield return new WaitForSeconds(time);
		Time.timeScale = 1;
	}

	private IEnumerator WarpAnimation()
	{
		yield return new WaitForSeconds(_WarpAnimationTime);
		if(_LevelEnded)
		{
            yield return StartCoroutine(fadeToBlack());
			if(Application.loadedLevel != Application.levelCount - 1)
			{
				Application.LoadLevel(Application.loadedLevel + 1);
			}
			else
			{
				Application.LoadLevel(0);
			}
		}
	}

    private IEnumerator fadeToBlack()
    {
        float fadeLenght = 1.0f, threshold = 0.1f;
        Image img = Camera.main.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>();

        while(img.color.a < 1)
        {
            img.color += new Color(0, 0, 0, Time.deltaTime / fadeLenght);
            if (img.color.a + threshold > 1)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
            }
            yield return null;
        }
        yield break;
    }

    private void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody2D>();
		_MagnetList = new List<GameObject>();
		_Animator = transform.GetChild(0).GetComponent<Animator>();
		_MeshOffset = transform.GetChild(0).localPosition;
		_StartPosition = transform.position;
		_LevelEnded = false;
	}

	private void Start()
	{
		_LevelStartTime = Time.time;
		transform.GetChild(2).GetComponent<ParticleSystem>().Play();
	}

	private void Update()
	{
		_Input = Input.GetAxis("Horizontal");
		if(!Input.GetButton("Fire1"))
		{
			for(int i=0; i < _MagnetList.Count; ++i)
			{
				_MagnetList[i].GetComponent<Magnet>().desactivate();
			}
		}

		if(Input.GetButtonDown("Submit"))
		{
			Respawn();
		}

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
		if(!_LevelEnded)
		{
			_LevelEndTime = Time.time;
		}

		if(_IsDead)
		{
			_DeathAnimationElapsed += Time.fixedDeltaTime;
			float shake = (_DeathAnimationElapsed / _DeathAnimationTime) * _MaxDeathShake;
			transform.GetChild(0).localPosition = _MeshOffset;
			if(_DeathAnimationElapsed > _DeathAnimationTime)
			{
				Respawn();
			}
			else
			{
				float shakeX = Random.Range(-shake, shake);
				float shakeY = Random.Range(-shake, shake);
				transform.GetChild(0).localPosition += new Vector3(shakeX, shakeY);
			}
			return;
		}

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
		transform.GetChild(0).localPosition = new Vector3(0.0f, -0.5f, 0.0f);
		if(_MasterMagnet != null)
		{
			transform.GetChild(0).localPosition = Vector3.zero;
			transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(_SuckedRotation, 90.0f, 0.0f));
			_SuckedRotation += _SuckedSpeed;
		}
		else if(Mathf.Abs(_Rigidbody.velocity.y) > 0.5f || !_IsOnGround)
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
		if(_WithCurve)
		{
			forceRatio = _MasterAttractionCurve.Evaluate(forceRatio);
		}

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
			if(_WithCurve)
			{
				forceRatio = _AttractionCurve.Evaluate(forceRatio);
			}
			
			float forceMode = (_AttractionImpulse) ? _AttractionImpulseForce : Mathf.Max(_AttractionForce, _AttractionForceCap);
			forceMode = _AttractionMinForce + (forceMode - _AttractionMinForce) * forceRatio;
			attractForce += distance * forceMode;

			_MagnetList[i].GetComponent<Magnet>().activate();
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
			transform.GetChild(1).GetComponent<ParticleSystem>().Play();
		}

		_Rigidbody.AddForce(repulsionForce, ForceMode2D.Impulse);
	}

	private void Respawn()
	{
		Camera.main.GetComponent<LerpCamera>()._ShakeValue = 0.0f;
		transform.position = _StartPosition;
		_Animator.Play("WalkBlend");
		_Rigidbody.velocity = Vector3.zero;
		_Rigidbody.isKinematic = false;
		ClearMagnets();
		_MasterMagnet = null;
		_IsDead = false;
		_LevelStartTime = Time.time;
		_LevelEnded = false;
		MovableMagnet.RespawnAllInstances();
		transform.GetChild(2).GetComponent<ParticleSystem>().Play();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Magnet" && _MasterMagnet == null)
		{
			_MagnetList.Add(collider.gameObject);
		}
		else if(collider.tag == "MasterMagnet")
		{
			ClearMagnets();
			StartCoroutine(WarpAnimation());
			_MasterMagnet = collider.gameObject;
			Camera.main.GetComponent<LerpCamera>()._ShakeValue = 0.1f;
			_Animator.SetBool("Sucked", true);
			_LevelEnded = true;
			_LevelEndTime = Time.time;
			float elapsed = _LevelEndTime - _LevelStartTime;
			if(LevelsManager.instance != null)
			{
				LevelsManager.instance.SubmitTimer(elapsed, Application.loadedLevel);
			}
		}
		else if(collider.tag == "Hazard" && !_IsDead)
		{
			transform.GetChild(3).GetComponent<ParticleSystem>().Play();
			_DeathAnimationElapsed = 0.0f;
			_Rigidbody.isKinematic = true;
			_IsDead = true;
			_Animator.SetTrigger("Dead");
			Camera.main.GetComponent<LerpCamera>()._ShakeValue = 0.01f;
			StartCoroutine(FreezeFrame(0.005f));
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag == "Magnet")
		{
			_MagnetList.Remove(collider.gameObject);
			collider.GetComponent<Magnet>().desactivate();
		}
		else if(collider.tag == "MasterMagnet")
		{
			_MasterMagnet = null;
			Camera.main.GetComponent<LerpCamera>()._ShakeValue = 0.0f;
			_Animator.SetBool("Sucked", false);
			_SuckedRotation = 0.0f;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.contacts.Length > 0)
		{
			if(collision.contacts[0].normal.y > 0.3f && _Rigidbody.velocity.y <= 0.0f)
			{
				_IsOnGround = true;
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.contacts.Length > 0)
		{
			if(collision.contacts[0].normal.y > 0.3f && _Rigidbody.velocity.y <= 0.0f)
			{
				_IsOnGround = true;
			}
		}
	}

	private void ClearMagnets()
	{
		for(int i=0; i<_MagnetList.Count; ++i)
		{
			_MagnetList[i].GetComponent<Magnet>().desactivate();
		}
		_MagnetList.Clear();
	}
}
