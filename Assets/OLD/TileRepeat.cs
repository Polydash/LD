using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileRepeat : MonoBehaviour {

    public float _maxDistance = 100.0f;
    [HideInInspector]
    public GameObject[] _spriteArray;

    private GameObject _next, _previous ;
    private int _index = 0;
    private Camera _myCamera;
    private Vector2 _offset;

    // Use this for initialization
    void Start () {
        _myCamera = Camera.main;
        _spriteArray = new GameObject[this.transform.childCount];
       for (int i = 0; i < this.transform.childCount; i++)
        {
            _spriteArray[i] = this.transform.GetChild(i).gameObject;
        }
        _next = _spriteArray[0];
        _previous = _spriteArray[_spriteArray.Length - 1];
        _offset = _spriteArray[1].transform.position - _spriteArray[0].transform.position;
    }
	
	// Update is called once per frame
	void Update () {

       // Debug.Log("Distance between the next sprite and the camera : " + (_myCamera.transform.position.x - _next.transform.position.x));

        if (_myCamera.transform.position.x - _next.transform.position.x > _maxDistance)
        {
            _next.transform.position = _previous.transform.position + new Vector3(_offset.x , _offset.y , 0);
            _index++;
            if (_index > _spriteArray.Length - 1)
                _index = 0;
           _previous = _next;
           _next = _spriteArray[_index];
        }
    }


}
