using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float _moveSpeed = 5.0f;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(_moveSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += new Vector3(-_moveSpeed * Time.deltaTime, 0, 0);

        }
    }
}
