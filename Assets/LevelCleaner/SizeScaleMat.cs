using UnityEngine;
using System.Collections;

public class SizeScaleMat : MonoBehaviour
{
	private void Awake()
	{
		float tileX = transform.localScale.x;
		float tileY = transform.localScale.y;
		GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(tileX, tileY));
	}
}
