using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

	[SerializeField] float time = 2;
	[SerializeField] string sceneToLoad;

	// Use this for initialization
	void Start () 
	{
		
		StartCoroutine( CountDown() );
	}
	
	
	
	//Simple Countdown 
	//After time interval a scene is loaded.
	IEnumerator CountDown()
	{
		yield return new WaitForSeconds( time );

		SceneManager.LoadScene( sceneToLoad );
	}


}
