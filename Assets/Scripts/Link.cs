using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour 
{

	public float borderWidth = 0.2f;
	public float lineThickness = 0.5f;
	public float scaleTime = 0.25f;

	public float delay = 0.1f;

	//public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

	private Transform _transform;

	void Awake()
	{
		_transform = transform;
	}

	void Start()
	{
		
	}

	public void DrawLink( Vector3 startPosition, Vector3 endingPosition )
	{
		
		Debug.Log("Drawing Link now....");
		//Starting scale for link z = 0 link is invisible
		_transform.localScale = new Vector3( lineThickness, 1f, 0f );

		//Get direction vector
		Vector3 directionVector = endingPosition - startPosition;

		Debug.Log( "Direction Vector: " + directionVector );

		// length of direction vector - borderwidth * 2
		//border width is subtracted to compensate for the hollow centre of the nodes.
		float zScale = directionVector.magnitude - borderWidth * 2;

		//Set the  new scale for the link . zscale is no 0 so link is now visible
		Vector3 newScale = new Vector3( lineThickness, 1, zScale );

		//Rotate link to match direction vector
		_transform.rotation = Quaternion.LookRotation( directionVector );

		//Shift link forward by border amount
		_transform.position = startPosition + ( _transform.forward * borderWidth );

		// iTween.ScaleTo( gameObject, iTween.Hash(

		// 	"time", scaleTime,
		// 	"scale", newScale,
		// 	"easetype", easeType,
		// 	"delay", delay

		// ));
	}




	
}
