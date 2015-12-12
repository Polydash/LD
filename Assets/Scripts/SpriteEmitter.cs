using UnityEngine;
using System.Collections;

public class SpriteEmitter : MonoBehaviour {

    public SpriteParticle _spriteParticle;
    public int _number = 5;
    public Vector2 _maxVelocity = Vector2.zero, _minVelocity = Vector2.zero;
    public float _minSpeed = 0, _maxSpeed = 0;
    public float _lifeTime = 0.0f;
    public bool _playOnStart = false;

    public int _velocityType = 0;


	// Use this for initialization
	void Start () {
        if (_playOnStart)
            play();
	}
	
	// Update is called once per frame
	void Update () {

        //debug
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            play();
        }
	}

    public void play()
    {
        for (int i = 0; i < _number; i++)
        {
            SpriteParticle obj = GameObject.Instantiate(_spriteParticle) as SpriteParticle;
            obj.transform.position = this.transform.position;

            Vector2  velocity = Vector2.zero;

            switch (_velocityType)
            {
                case (0)://Constant
                    velocity = Random.Range(_minSpeed, _maxSpeed) * new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                    break;
                case (1 )://Between 2 vectors
                    velocity = new Vector2(Random.Range(_minVelocity.x, _maxVelocity.x), Random.Range(_minVelocity.y, _maxVelocity.y));
                    break;
            }

            obj.velocity = velocity;
            Destroy(obj.gameObject, _lifeTime);
        }
    }
}
