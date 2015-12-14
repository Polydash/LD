using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	private static MusicManager instance = null;
	private float _FadeInValue;
	private AudioSource _Source;
	private int previousLevel = 0;

	public AudioClip _MenuTheme;
	public AudioClip _GameTheme;
	public float _FadeTime;

	private void Awake()
	{
		_Source = GetComponent<AudioSource>();
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}
	}

	private void Update()
	{
		_FadeInValue += Time.deltaTime / _FadeTime;
		_FadeInValue = Mathf.Min(1.0f, _FadeInValue);
		_Source.volume = _FadeInValue;

		transform.position = Camera.main.transform.position;
	}

	private void OnLevelWasLoaded(int level)
	{
		if(level == 0)
		{
			if(previousLevel > 0)
			{
				previousLevel = 0;
				_FadeInValue = 0;
				_Source.clip = _MenuTheme;
				_Source.Play();
			}
		}
		else
		{
			if(previousLevel == 0)
			{
				previousLevel = level;
				_FadeInValue = 0;
				_Source.clip = _GameTheme;
				_Source.Play();
			}
		}
	}
}
