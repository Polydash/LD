using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {


    public float _rotationSpeed = 50.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.eulerAngles += new Vector3(0, 0, _rotationSpeed * Time.deltaTime);
	}
}
