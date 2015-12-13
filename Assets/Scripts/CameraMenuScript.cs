using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMenuScript : MonoBehaviour {

    public Vector3 _target;
    public float _lerpSpeed = 1.0f, _threshold = 0.01f;
    public List<Vector3> _positionList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if ((_target - this.transform.position).magnitude > _threshold)
            this.transform.position = Vector3.Lerp(this.transform.position, _target, _lerpSpeed);
        else
            this.transform.position = _target;
    }

    public void goTo(int index)
    {
        _target = _positionList[index];
    }
}
