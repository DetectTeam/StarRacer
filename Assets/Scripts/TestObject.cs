using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarRacer;

public class TestObject : MonoBehaviour 
{

	[SerializeField] private int starValue = 0;

	void OnMouseDown()
    {
        Debug.Log( "Clicked " + gameObject.name );
		
		StartCoroutine( "StarSequence" );
    }

	IEnumerator StarSequence()
	{
		Debug.Log( "Starting Star Sequence" );
		
		yield return new WaitForSeconds( 0.25f );

		//Check if last star clicked is one less than current star
		if( StarManager.previousStar == ( starValue - 1 ) )
		{
			Debug.Log( "Valid: Draw a line between these two points." );

			if( StarManager.previousStar != -1 )
			{
				//Get reference to previous star

				
			}

			//Update the previous star value with this star
			StarManager.previousStar = starValue;
		}
		else
		{
			Debug.Log( "Invalid: Not the correct Sequence" );
		}
		
		



		//if valid star trigger success tween

		//if valid star trigger line between previous star and this star

		//if invalid star trigger failure tween.

		Debug.Log( "Ending Star Sequence" );
	}


	

}
