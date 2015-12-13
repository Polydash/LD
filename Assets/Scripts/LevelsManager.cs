using UnityEngine;
using System.Collections;
using System.IO;

public class LevelsManager : MonoBehaviour
{
	public static LevelsManager instance = null;

	private float[] _Timers;

	private void Awake()
	{
		_Timers = new float[Application.levelCount - 1];
		for(int i = 0; i < Application.levelCount - 1; ++i)
		{
			_Timers[i] = -1.0f;
		}
	}

	public bool SubmitTimer(float timer, int level)
	{
		if(_Timers[level] < 0.0f || _Timers[level] > timer)
		{
			_Timers[level] = timer;
			return true;
		}

		return false;
	}
}
