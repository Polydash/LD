using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
	private Text _Text;
	private PlayerMove _Player;

	private void Awake()
	{
		_Text = GetComponent<Text>();
		_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
	}

	private void Update()
	{
		float elapsed = _Player._LevelEndTime - _Player._LevelStartTime;
		string display = "Left mouse / Button A : Attract\nRight mouse / Button X : Repulse\nR / Start : Restart level\nTime : " + elapsed.ToString("0.00") + "\n";
		if(LevelsManager.instance != null)
		{
			float bestTime = LevelsManager.instance.GetTimer(Application.loadedLevel - 1);
			if(bestTime > 0.0f)
			{
				display += "Best time : " + bestTime.ToString("0.00");
			}
		}

		_Text.text = display;
	}
}
