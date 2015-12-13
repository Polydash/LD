using UnityEngine;
using System.Collections;

public class QuadInstancer : MonoBehaviour
{
	public GameObject _QuadPrefab;

	private void Awake()
	{
		transform.position = Vector3.zero;
		SpriteRenderer[] sprites = GameObject.FindObjectsOfType<SpriteRenderer>();

		for(int i=0; i<sprites.Length; ++i)
		{
			GameObject quad = Instantiate<GameObject>(_QuadPrefab);
			quad.transform.position = sprites[i].transform.position;
			quad.transform.rotation = sprites[i].transform.rotation;
			quad.transform.localScale = sprites[i].transform.localScale;

			quad.transform.parent = transform.GetChild(0).transform;
		}
	}


}
