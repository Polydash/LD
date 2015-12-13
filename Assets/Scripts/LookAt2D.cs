using UnityEngine;
using System.Collections;

public class LookAt2D : MonoBehaviour {

    public float _offset;
    public float radius;

    private Vector3 _initialScale;
    private GameObject _target;
    private Magnet _parentMagnet;

    // Use this for initialization
    void Start () {
        _parentMagnet =  this.transform.parent.GetComponent<Magnet>();
        this.GetComponent<ParticleSystem>().enableEmission = _parentMagnet.isActive();
        _target = GameObject.FindGameObjectWithTag("Player");
        _initialScale = this.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        if (_parentMagnet.isActive())
        {
            float distance = new Vector2(this.transform.position.x - _target.transform.position.x, this.transform.position.y - _target.transform.position.y).magnitude;
            this.transform.localScale = new Vector3(distance / radius, _initialScale.y, _initialScale.z); ;

            Vector3 dir = _target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + _offset;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
