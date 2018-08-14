using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour 
{

	[SerializeField] private GameObject line;

	private Vector2 mousePosition;
	private Vector2 oldMousePosition = new Vector2( 100.0f , 100.0f );

	private bool isFirst;

	void Update()
	{
		//Left Mouse Button Clicked....
		if( Input.GetMouseButtonDown( 0 ) )
		{
			mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			
			if( oldMousePosition != mousePosition )
			{
				Instantiate( line, mousePosition, Quaternion.Euler( 0.0f, 0.0f, 0.0f ) );
			}
			else
			{
				Debug.Log( "positions are equal...." );
			}

			oldMousePosition = mousePosition;
		}
	} 

}
