using UnityEngine;
using System.Collections;
using System.IO;

public class LevelsManager : MonoBehaviour
{
	public static LevelsManager instance = null;

	private float[] _Timers;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}

		_Timers = new float[Application.levelCount - 1];
		LoadFile();
	}

	public float GetTimer(int level)
	{
		return _Timers[level];
	}

	public void SubmitTimer(float timer, int level)
	{
		if(_Timers[level] < 0.0f || _Timers[level] > timer)
		{
			_Timers[level] = timer;
			SaveFile();
		}
	}

	private void LoadFile()
	{
		if(File.Exists("Polaris.sav"))
		{
			string[] lines = File.ReadAllLines("Polaris.sav");
			for(int i=0; i < Application.levelCount - 1; ++i)
			{
				_Timers[i] = float.Parse(lines[i]);
			}
		}
		else
		{
			for(int i = 0; i < Application.levelCount - 1; ++i)
			{
				_Timers[i] = -1.0f;
			}
		}
	}

	private void SaveFile()
	{
		string[] lines = new string[Application.levelCount - 1];
		for(int i=0; i < Application.levelCount - 1; ++i)
		{
			lines[i] = _Timers[i].ToString("0.00");
		}
		File.WriteAllLines("Polaris.sav", lines);
	}

	public float GetCumulativeTime()
	{
		float cumulative = 0.0f;
		for(int i=0; i < Application.levelCount - 1; ++i)
		{
			if(_Timers[i] < 0.0f)
			{
				return -1.0f;
			}
			cumulative += _Timers[i];
		}

		return cumulative;
	}
}
