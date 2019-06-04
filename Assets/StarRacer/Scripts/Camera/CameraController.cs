using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{


	Camera mainCamera;
	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main;
		#if UNITY_IOS
      		
			//Adjust Camera Size  
			Debug.Log( "IOS " + SystemInfo.deviceName );
			mainCamera.orthographicSize = 27;  
		
    	#endif

	}
	
	
}
