using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMenuScript : MonoBehaviour {

    public Vector3 _target;
    public float _lerpSpeed = 1.0f, _threshold = 0.01f;
    public List<Vector3> _positionList;
    public List<GameObject> _defaultButtonList;
    private int _currentIndex = 0;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if ((_target - this.transform.position).magnitude > _threshold)
            this.transform.position = Vector3.Lerp(this.transform.position, _target, _lerpSpeed);
        else
            this.transform.position = _target;

        if((Input.GetButton("Horizontal") || Input.GetButton("Vertical")  )&& UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_defaultButtonList[_currentIndex]);
        }
    }

    public void goTo(int index)
    {
        _target = _positionList[index];
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_defaultButtonList[index]);
        _currentIndex = index;
    }
}
