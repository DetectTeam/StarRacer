using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCounter : MonoBehaviour 
{
	private static LevelCounter _instance;
   	public static LevelCounter Instance { get { return _instance; } }
	   
	[SerializeField] private int levelCount;
	public int LevelCount { get{ return levelCount; } set{ levelCount = value; } }
	
	[SerializeField] private int levelMax;

	private void Awake()
	{ 
		 DontDestroyOnLoad(this.gameObject);
	}

	private void OnEnable()
	{
		Messenger.AddListener( "IncreaseLevel", IncrementLevel );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "IncreaseLevel", IncrementLevel );
	}

	public void IncrementLevel()
	{
		Debug.Log( "Increasing Level" );
		levelCount ++;
		//LevelCheck();
	}

	public void LevelCheck()
	{
		if( levelCount >= levelMax )
		{
			Debug.Log( "End Game...." );
			levelCount = 0;
		}
	}

}
