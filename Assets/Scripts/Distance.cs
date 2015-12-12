using UnityEngine;
using System.Collections;

public class Distance : MonoBehaviour {

	public bool _useFactor = false;
	public float _distanceFactor = 1.0f;


	private Vector3 _offset;
	private Camera _myCamera ;

	// Use this for initialization
	void Start () {
		_offset = this.transform.position;
		_myCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
			this.transform.position = new Vector3 (_myCamera.transform.position.x * _distanceFactor,_myCamera.transform.position.y * _distanceFactor, 0) + _offset;
	}
}
