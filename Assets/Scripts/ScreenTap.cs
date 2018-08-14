using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Attach to transparent UI Panel
//Loads a new sceene when panel is clicked

public class ScreenTap : MonoBehaviour 
{
	[SerializeField] private string sceneToLoad;
	

	//Called from ScreenTap Panel OnClick Event
	public void ChangeScene()
	{
		if( sceneToLoad != null || sceneToLoad.Length > 0 )
			SceneManager.LoadScene( sceneToLoad );
	}
	
}
