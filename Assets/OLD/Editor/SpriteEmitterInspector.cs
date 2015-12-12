using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(SpriteEmitter))]
public class SpriteEmitterInspector : Editor {


    int selected;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnInspectorGUI()
    {
        SpriteEmitter myTarget = (SpriteEmitter)target;

        myTarget._spriteParticle = (SpriteParticle) EditorGUILayout.ObjectField("Particle :" , myTarget._spriteParticle, typeof(SpriteParticle), true );
        myTarget._number = EditorGUILayout.IntField("Number of Particles :", myTarget._number);
        myTarget._lifeTime = EditorGUILayout.FloatField("Lifetime :", myTarget._lifeTime);

        string[] velocityOptions = new string[]{"Radial", "Random between 2 vectors"};
        myTarget._velocityType = EditorGUILayout.Popup("Directions :", myTarget._velocityType, velocityOptions);

        switch (myTarget._velocityType)
        {
            case (0):
                myTarget._minSpeed = EditorGUILayout.FloatField(" Min speed : ", myTarget._minSpeed);
                myTarget._maxSpeed = EditorGUILayout.FloatField(" Max speed : ", myTarget._maxSpeed);
                break;
            case (1):
                myTarget._minVelocity = EditorGUILayout.Vector2Field(" Min velocity : ", myTarget._minVelocity);
                myTarget._maxVelocity = EditorGUILayout.Vector2Field(" Max velocity : ", myTarget._maxVelocity);
                break;
        }
       
        

        myTarget._playOnStart = EditorGUILayout.Toggle("Play on start :", myTarget._playOnStart);


        //base.OnInspectorGUI();
    }
}
