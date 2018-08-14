using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; //JSON NET Plugin

public class JSONManager : MonoBehaviour 
{

	public static string ObjToJson( System.Object obj )
	{

		string jsonString = "";

		if( obj != null )
		{
			jsonString = JsonConvert.SerializeObject( obj );
		}
		else
		{
			Debug.Log( "No Valid Object Found..." );
		}

		return jsonString;
		
	}
	
}
