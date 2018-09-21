using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;






public class DemoTimer : MonoBehaviour 
{
	[SerializeField] private int countDown;
	[SerializeField] private float delay = 1.0f;

	[SerializeField] private string sceneToLoad; 

	void Start()
	{
		StartCoroutine( "CountDown" );
	}

	IEnumerator CountDown()
	{
		while( countDown > 0 )
		{
			yield return new WaitForSeconds( delay );	
			countDown --;
		}

		SceneManager.LoadSceneAsync( sceneToLoad );
	}

}
