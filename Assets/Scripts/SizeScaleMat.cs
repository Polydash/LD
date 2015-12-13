using UnityEngine;
using System.Collections;

public class SizeScaleMat : MonoBehaviour
{
	private void Awake()
	{
		for(int i=0; i<transform.childCount; ++i)
		{
			Transform child = transform.GetChild(i).transform;
			float tileX = child.localScale.x;
			float tileY = child.localScale.y;
			child.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(tileX, tileY));
			child.GetComponent<Renderer>().material.SetTextureScale("_BumpMap", new Vector2(tileX, tileY));
		}
	}
}
