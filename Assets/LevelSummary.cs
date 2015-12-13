using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelSummary : MonoBehaviour
{
	private Text _TextComponent;

	private void Awake()
	{
		_TextComponent = GetComponent<Text>();
	}

	private void Update()
	{
		GameObject selection = EventSystem.current.currentSelectedGameObject;
		if(selection != null)
		{
			if(selection.tag == "LevelIcon")
			{
				int index = selection.GetComponent<LevelIcon>()._LevelIndex - 1;
				UpdateText(index);
			}
			else
			{
				UpdateText(-1);
			}
		}
		else
		{
			UpdateText(-1);
		}
	}

	private void UpdateText(int i)
	{
		string display = "All levels: ";

		float cumulative = LevelsManager.instance.GetCumulativeTime();
		if(cumulative > 0.0f)
		{
			display += cumulative.ToString("0.00") + "\n\n";
		}
		else
		{
			display += "???\n\n";
		}

		if(i >= 0)
		{
			display += "Level " + (i+1).ToString() + "\n";
			display += "Level time:";
			float bestTime = LevelsManager.instance.GetTimer(i);
			if(bestTime > 0.0f)
			{
				display += bestTime.ToString("0.00") + "\n";
			}
			else
			{
				display += "???";
			}
		}

		_TextComponent.text = display;
	}
}
