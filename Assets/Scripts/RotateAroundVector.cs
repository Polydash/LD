using UnityEngine;
using System.Collections;

public class RotateAroundVector : MonoBehaviour
{
	public Vector3 _SpeedAngles;

	private void Update()
	{
		transform.eulerAngles += _SpeedAngles * Time.deltaTime;
	}
}
