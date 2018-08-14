using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLine : MonoBehaviour
{
	[SerializeField] private float rotationSpeed;
	[SerializeField] private float maxPoints;

	private VectorLine line;

	// Use this for initialization
	void Start () {
		SetLine();
	}

	void SetLine()
	{
		//Make sure line is Destroyed
		VectorLine.Destroy( ref line );

		line = new VectorLine( "Line", new List<Vector2>(), 2, LineType.Continuous, Joins.Fill );
		line.drawTransform = transform;
		line.collider = true;
	}

	void Update()
	{
		// Since we can rotate the transform, get the local space for the current point, so the mouse position won't be rotated with the line
		var mousePos = transform.InverseTransformPoint ( Input.mousePosition );
	
		// Add a line point when the mouse is clicked
		if ( Input.GetMouseButtonDown ( 0 ) ) 
		{
			line.points2.Add ( mousePos );
		
			// Start off with 2 points
			if ( line.points2.Count == 1 ) {
				line.points2.Add ( Vector2.zero );
			}
		
			if ( line.points2.Count == maxPoints ) 
			{
				//endReached = true;
			}
	}
	
	// The last line point should always be where the mouse is; only draw when there are enough points
	if ( line.points2.Count >= 2 ) 
	{
		line.points2[ line.points2.Count-1 ] = mousePos;
		line.Draw();
	}
	
	// Rotate around midpoint of screen
	transform.RotateAround ( new Vector2( Screen.width/2, Screen.height/2 ) , Vector3.forward, Time.deltaTime * rotationSpeed * Input.GetAxis ( "Horizontal" ) );
	
	}
	
	
}
