using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour 
{

	private Camera cam;

	// Use this for initialization
	void Start () 
	{
		cam = GetComponent<Camera>();

		if( !cam )
		{
			Debug.Log( "Camera not found in scene." );
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Input.GetMouseButtonDown( 0 ) )
		{ 	
			// if left button pressed...
     		Ray ray = cam.ScreenPointToRay( Input.mousePosition );
     		RaycastHit hit;
     
	 		if ( Physics.Raycast(ray, out hit ) )
			{
       			// the object identified by hit.transform was clicked
       			// do whatever you want
				Debug.Log( hit.transform.name );   
     		}
  		}

	}
}
