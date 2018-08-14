using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour 
{

	private LineRenderer line;
	private Vector2 mousePosition;
	private Vector2 oldMousePosition = new Vector2( 1000.0f, 1000.0f );

	[SerializeField] private bool simplifyLine = false;
	[SerializeField] float simplifyTolerance = 0.02f;

	// Use this for initialization
	void Start () 
	{
		line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if( Input.GetMouseButton( 0 ) )
		{
			mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );

			if( oldMousePosition != mousePosition )
			{
				line.positionCount++;
				line.SetPosition( line.positionCount - 1, mousePosition );
			}

			oldMousePosition = mousePosition;
		}


		if( Input.GetMouseButtonUp( 0 ) )
		{
			if( simplifyLine )
			{
				line.Simplify( simplifyTolerance );
			}

			enabled = false;

		}

	}
}
