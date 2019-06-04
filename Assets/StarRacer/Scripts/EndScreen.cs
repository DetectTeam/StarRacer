using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.PostProcessing;


namespace StarRacer
{

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
				//Shrink before making active
				endScreenBackground.transform.localScale = new Vector3( 0.01f, 0.01f, 0.01f );
				endScreenBackground.SetActive( true );
				
				//Scale back to 1 to 1 ratio
				iTween.ScaleTo ( endScreenBackground, iTween.Hash ("scale", new Vector3 (1.0f,1.0f,1.0f), "speed", speed, "easetype", "linear"));

				yield return new WaitForSeconds( 0.3f );

				//Add punch after scale is complete
				iTween.PunchScale( endScreenBackground, iTween.Hash( "x",+.3, "y",+.3, "time",1.0f));

				yield return new WaitForSeconds( 1.25f );

				//Pause Game
				Time.timeScale = 0.0f;
		}	
	}
}
