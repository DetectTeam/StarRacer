using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour 
{
	public void IncrementLevel()
	{
		Debug.Log( "Increasing Level" );
		//Messenger.Broadcast( "IncreaseLevel" );
	}	
}
