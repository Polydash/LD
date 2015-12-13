using UnityEngine;
using System.Collections;

public class ExitManager : MonoBehaviour
{
	private static ExitManager instance = null;

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
	}

	private void Update()
	{
		if(Input.GetButtonDown("Cancel"))
		{
			if(Application.loadedLevel > 0)
			{
				Application.LoadLevel(0);
			}
			else
			{
 				Application.Quit();
			}
		}
	}
}
