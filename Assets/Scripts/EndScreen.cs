using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.PostProcessing;


public class EndScreen : MonoBehaviour 
{
	
	[SerializeField] private float speed = 6.0f;
	[SerializeField] private GameObject endScreenBackground;


	// [SerializeField] private PostProcessingProfile blurProfile;
	// [SerializeField] private PostProcessingProfile defaultProfile;

	// public PostProcessingBehaviour cameraPostProcess;


	// public void EnableCameraBlur( bool state )
	// {
	// 	if( cameraPostProcess != null && blurProfile != null && defaultProfile != null  )
	// 	{
	// 		cameraPostProcess.profile = ( state ) ? blurProfile : defaultProfile;
	// 	}
	// }

	public void ActivateEndBackground()
	{
		StartCoroutine( "IEActivateEndBackground" );
	}

	IEnumerator IEActivateEndBackground()
	{
			Debug.Log( "END SCREEN ON ENABLE CALLED" );
			endScreenBackground.SetActive( true );
			iTween.ScaleTo ( endScreenBackground, iTween.Hash ("scale", new Vector3 (1.0f,1.0f,1.0f), "speed", speed, "easetype", "linear"));

			yield return new WaitForSeconds( 0.3f );

			//Punch animation when correct star is encountered
		    iTween.PunchScale( endScreenBackground, iTween.Hash( "x",+.3, "y",+.3, "time",1.0f));



	}

	
}
