using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Move an object along path of waypoints using iTween */
public class PathFollow : MonoBehaviour 
{

	[SerializeField] private Transform[] waypointArray; //Array of waypoints that make up the path

	[SerializeField] private float percentPerSecond = 0.02f; // 2% per second
	[SerializeField] private float currentPathPercent = 0.0f; //Percnetage of the path completed. 0 - 1

	DrawLineDemo drawLineDemo;

	void Start()
	{
		drawLineDemo = GetComponent<DrawLineDemo>();

		if( drawLineDemo )
		{
			StartCoroutine( Delay() );
		}
		
	}

	// Update is called once per frame
	void Update () 
	{
		currentPathPercent += percentPerSecond * Time.deltaTime; //Calculate the percentage of the path completed
		
		if( currentPathPercent <= 1 )
			iTween.PutOnPath( this.gameObject, waypointArray, currentPathPercent ); //Use itween to move object along the path

	}

	//Draw the path onscreen.
	//For testing only.
	void OnDrawGizmos()
	{
		iTween.DrawPath( waypointArray );
	}

	IEnumerator Delay()
	{
		yield return new WaitForSeconds( 0.62f );
		
		drawLineDemo.enabled = true;	
	}
}
