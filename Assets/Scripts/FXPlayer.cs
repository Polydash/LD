using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FXPlayer : MonoBehaviour
{
	[System.Serializable]
	public struct AudioInfo
	{
		public string name;
		public AudioClip clip;
		public float volume;
		public bool looping;
	}

	public AudioInfo[] _FX;

	private AudioSource _Source;

	private void Awake()
	{
		_Source = GetComponent<AudioSource>();
	}

	public void Play(string name)
	{
		for(int i=0; i<_FX.Length; ++i)
		{
			if(_FX[i].name.Equals(name))
			{
				_Source.clip = _FX[i].clip;
				_Source.volume = _FX[i].volume;
				_Source.loop = _FX[i].looping;
				_Source.Play();
			}
		}
	}

	public bool IsPlaying(string name)
	{
		for(int i=0; i<_FX.Length; ++i)
		{
			if(_FX[i].name.Equals(name))
			{
				if(_Source.clip == _FX[i].clip && _Source.isPlaying)
					return true;
				else
					return false;
			}
		}

		return false;
	}

	public void StopIfPlaying(string name)
	{
		if(IsPlaying(name))
		{
			_Source.Stop();
		}
	}

	public void Stop()
	{
		_Source.Stop();
	}
}
