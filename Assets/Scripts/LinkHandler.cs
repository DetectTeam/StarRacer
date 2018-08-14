using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class LinkHandler : MonoBehaviour 
{

	public GameObject linkPrefab;
	//private Transform _transform;

	void Start()
	{
		//_transform = transform;
		VectorLine.canvas.sortingOrder = -1;

		VectorLine.SetCanvasCamera (Camera.main);
	}

	
	public void InitLink( Vector2 source, Vector2 dest )
	{
		Debug.Log( "Vector Line" );
		Debug.Log( source + " " + dest );

		var linePoints = new List<Vector2>(){ source, dest };

		var myLine = new VectorLine("Line", linePoints, 0.1f); 

		myLine.Draw();
		
		//VectorLine.SetLine( Color.red, source, dest );
		
		//LinkStars( source, dest );
	}

}
