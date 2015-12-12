using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

public class SpriteParticle : MonoBehaviour {

    public Vector2 velocity = Vector2.zero;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0);
	}
}
