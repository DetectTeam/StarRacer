using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCounter : MonoBehaviour 
{
	[SerializeField] private int levelCount;
	[SerializeField] private int levelMax;

	private void OnEnable()
	{
		Messenger.AddListener( "IncreaseLevel", LevelCheck );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "IncreaseLevel", LevelCheck );
	}

	public void LevelCheck()
	{
		levelCount++;

		if( levelCount >= levelMax )
		{
			Debug.Log( "End Game...." );
			levelCount = 0;
		}

	}
}
