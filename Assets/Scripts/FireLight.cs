using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Light))]
public class FireLight : MonoBehaviour {

    public AnimationCurve _intensityVariation;
    public float _factor = 1;

    private Light _light;
	// Use this for initialization
	void Start () {
        _light = this.GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        _light.intensity = _intensityVariation.Evaluate( Time.time )* _factor;
    }
}
