using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour 
{
	void Start()
	{
		Debug.Log( "Touch Handler Working... " + Input.touchSupported );
	}

	void Update()
	{
		// foreach( Touch touch in Input.touches )
		// {
			
		// 	switch( touch.phase )
		// 	{
		// 		case TouchPhase.Began:
		// 			Debug.Log( "Touch Began" );
        //       	break;

		// 		case TouchPhase.Moved:
		// 			Debug.Log( "Touch Moved" );
        //       	break;

		// 		case TouchPhase.Stationary:
		// 			Debug.Log( "Touch Stationary" );
        //       	break;  

		// 		case TouchPhase.Ended:
		// 			Debug.Log( "Touch Ended" );
        //       	break;

		// 		case TouchPhase.Canceled:
		// 			Debug.Log( "Touch Canceled" );
        //       	break;

		// 	}
		// }
	}
	
}
