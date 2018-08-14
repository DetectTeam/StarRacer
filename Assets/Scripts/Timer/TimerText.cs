using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour 
{

	[SerializeField] private TextMeshProUGUI timerText;

	

	public void UpdateTimerText( string currentTime )
	{
	   timerText.text = currentTime;
	}
}
