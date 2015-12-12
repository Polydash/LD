using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	public Transform _Player;

	private void Update()
	{
		transform.position = _Player.position;
		transform.position += new Vector3(0.0f, 0.0f, -10.0f);
	}
}
