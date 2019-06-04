using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour 
{
	// Use this for initialization
	[SerializeField] private string sceneToLoad;
	
	//Called from ScreenTap Panel OnClick Event
	public void ChangeScene()
	{
		Debug.Log( "Changing Scene" );
		if( sceneToLoad != null || sceneToLoad.Length > 0 )
			SceneManager.LoadScene( sceneToLoad );	
	}
}
