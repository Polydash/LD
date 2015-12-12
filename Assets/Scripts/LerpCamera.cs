using UnityEngine;
using System.Collections;

public class LerpCamera : MonoBehaviour
{
	public Transform _Player;
	public float _Lerp;

	private void Update()
	{
		transform.position += (_Player.position - transform.position) * _Lerp;
 		transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
	}
}
