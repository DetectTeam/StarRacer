using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

	public void DeletePlayerPrefs()
	{
		PlayerPrefs.DeleteKey( "LevelCount" );
		PlayerPrefs.DeleteAll();
	}
}
