using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Set List of stars to Active
//Use Itween punch to scale stars for a nice effect

public class StarScaler : MonoBehaviour 
{
	[SerializeField] private float delay = 0.3f;

	[SerializeField] private List<GameObject> stars = new List<GameObject>(); 

	// Use this for initialization

	
	public IEnumerator ScaleStars () 
	{
		Time.timeScale = 1.0f;
		
		foreach( GameObject star in stars )
		{
		    //Punch animation when correct star is encountered
			Messenger<int>.Broadcast( "RandomizePitch" , 2 );
			star.SetActive( true );
				
			iTween.PunchScale( star, iTween.Hash( "x",-1.5, "y",-1.5, "time",0.75f ) );
			
			yield return new WaitForSeconds( delay );
		}
	}
	
	
}
