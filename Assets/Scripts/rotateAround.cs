using UnityEngine;
using System.Collections;

public class rotateAround : MonoBehaviour {


    public float _maxX, _maxY, _speed;
    private Vector3 _initialPosition;

	// Use this for initialization
	void Start () {
        _initialPosition = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
       // Debug.Log("Cos : " + Mathf.Cos(Time.time) + "  Sin : " + Mathf.Sin(Time.time));
        this.transform.position = _initialPosition + new Vector3( Mathf.Cos(Time.time * _speed) * _maxX, Mathf.Sin(Time.time * _speed) * _maxY , 0) ;
	}
}
