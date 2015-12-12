using UnityEngine;
using System.Collections;

public class VelocityScale : MonoBehaviour {

    public float _scaleFactor = 1.0f;
    public bool _useMaxScale = false;
    public float _maxScale = 2.0f;

    private Camera _myCamera;
    private Vector3 _initialScale;
    private float _scaleGoal;

    // Use this for initialization
    void Start () {
        _myCamera = Camera.main;
        _initialScale = this.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {

        _scaleGoal = _initialScale.x + _myCamera.velocity.magnitude * _scaleFactor;
        //Debug.Log("Camera velocity = " + _myCamera.velocity.magnitude);

        this.transform.localScale = new Vector3(Mathf.Lerp(this.transform.localScale.x, _scaleGoal, 0.3f), _initialScale.y, _initialScale.z);
    }
}
