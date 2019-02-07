using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarRacer;

public class StarContainer : MonoBehaviour 
{

	[SerializeField] private GameObject[] stars;

	// Use this for initialization
	private void Start () 
	{	

		//GameManager.Instance.Resume();
		
		var proximityHandler = GetComponent<ProximityHandler>();
		
		SessionManager.Instance.CreateSession();
		//yield return new WaitForSeconds( 0.25f );

		foreach( GameObject g in stars )
		{
			g.SetActive( true );
		}

		//yield return new WaitForSeconds( 0.25f );

		Messenger.Broadcast( "ProximityCheck" );

			
	}
	
	
}
