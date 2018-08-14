using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLineDemo : MonoBehaviour 
{

	[SerializeField] private bool isShipMoving;

	private Vector3 curPos;
	private Vector3 lastPos;

	[SerializeField] private Texture2D lineTex;

	[SerializeField] private int maxPoints = 5000;
	[SerializeField] private float lineWidth = 2.0f;

	[SerializeField] private bool line3D = false;


	[SerializeField] private VectorLine line;

	void Start()
	{
		VectorLine.canvas.sortingOrder = 20;
		
		if (line3D) 
		{
		
			line = new VectorLine("Line3D", new List<Vector3>(), lineTex, lineWidth, LineType.Continuous, Joins.Weld);
		}
		else 
		{
			line = new VectorLine("Line2D", new List<Vector2>(), lineTex, lineWidth, LineType.Continuous, Joins.Weld);		
		}

		//line.collider = true;
		line.Draw();

		StartCoroutine( "AddPointsToLine" );
	}

	

	IEnumerator AddPointsToLine()
	{
		Debug.Log( "Add Points to Line..." );
		while( true )
		{
			
			curPos = transform.position;
			yield return new WaitForSeconds(0.05f);
			lastPos = transform.position;
			
			if( lastPos != curPos )
			{
			
				if ( line3D ) 
				{
					line.points3.Add ( transform.position );
				}
				else 
				{
					line.points2.Add ( transform.position );
				}

				line.Draw();
			}
		}
		//Add point to line....
	}
	
}
