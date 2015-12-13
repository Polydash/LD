using UnityEngine;
using System.Collections;

public class CameraLerp : MonoBehaviour {

    public Vector3 _target;
    public float _lerpSpeed = 1.0f, _threshold = 0.01f;

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
}
