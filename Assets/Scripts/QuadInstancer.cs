using UnityEngine;
using System.Collections;

public class QuadInstancer : MonoBehaviour
{
	public GameObject _QuadPrefab;

	private void Start()
	{
		transform.position = Vector3.zero;
		SpriteRenderer[] sprites = GameObject.FindObjectsOfType<SpriteRenderer>();

		for(int i=0; i<sprites.Length; ++i)
		{
			GameObject quad = Instantiate<GameObject>(_QuadPrefab);

			quad.transform.position = new Vector3(Mathf.RoundToInt(sprites[i].transform.position.x),
				Mathf.RoundToInt(sprites[i].transform.position.y),
				Mathf.RoundToInt(sprites[i].transform.position.z));

			quad.transform.localScale = new Vector3(Mathf.RoundToInt(sprites[i].transform.localScale.x),
				Mathf.RoundToInt(sprites[i].transform.localScale.y),
				Mathf.RoundToInt(sprites[i].transform.localScale.z));

			quad.transform.rotation = sprites[i].transform.rotation;
			quad.transform.parent = transform.GetChild(0).transform;
		}
	}
}
