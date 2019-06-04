using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace StarRacer
{

	public class DemoManager : MonoBehaviour 
	{

		[SerializeField] private GameObject demoText; //Intro text for current demo
		[SerializeField] private GameObject stars; //The star layout for current demo

		[SerializeField] private GameObject demoPointer;

		[SerializeField] private  Image fader;



		private TeleType teleType;
		private StarScaler starScaler; 

		private Color tmpAlpha; 

		

		// Use this for initialization
		void Start () 
		{
			teleType = demoText.GetComponent<TeleType>();
			starScaler = stars.GetComponent<StarScaler>();
			
			
			
			if( !demoText )
			{
				Debug.Log( "DemoText Game Object not set..." );
			}
			
			StartCoroutine( "DemoSequence" );


		}
		
		private IEnumerator DemoSequence()
		{
			float tmp = 0;
			yield return StartCoroutine( teleType.RevealText() ); //Display Demo Text

			yield return new WaitForSeconds( 0.5f );

			yield return StartCoroutine( starScaler.ScaleStars() ); //Display Demo Stars
				
			yield return new WaitForSeconds( 3.0f );

			//Activate the pointer which will trace a path through the stars
			demoPointer.SetActive( true ); 


			//Return to Intro screen
			yield return new WaitForSeconds( 15.0f );

			
			while( fader.color.a < 1 )
			{
				
				yield return new WaitForSeconds( 0.05f );
				tmp = tmp + 0.05f;
				tmpAlpha.a = tmp;
				fader.color = tmpAlpha;
			}


			SceneManager.LoadSceneAsync( "Star_Racer_Intro" );




		}
	}
}
