﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; //Json Library
using System.IO;
using UnityEngine.Networking;


namespace StarRacer
{
	public class FileUploadHandler : MonoBehaviour 
	{
		
		private static FileUploadHandler _instance;
		public static FileUploadHandler Instance { get { return _instance; } }
		
		private string path = "";
		private string destinationPath;

		private string jsonString;

		private static readonly string PutSessionURL = "https://murmuring-fortress-76588.herokuapp.com/api/starracer/session";
		//private static readonly string PutSessionURL = "http://localhost:5000/api/starracer/session";

		private void Awake()
		{
			if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} 
				else 
				{
					_instance = this;
				}
		}

		private void Start()
		{
			
			path = GetPath() + "upload/";

			destinationPath = GetPath() + "sent/";
			Debug.Log( path );

			CheckDirectoryExists( path );
				
		}

		public void PUT( string jsonStr )
		{
			StartCoroutine( IEPUT( jsonStr ) );
		}

		//Send Put Request to the web server
		//Send the session data as a json string.
		private IEnumerator IEPUT( string jsonStr )
		{

			Debug.Log( "Uploading...Data" );
			Debug.Log( jsonStr);

			if( jsonStr.Length <= 0 )
			{
				Debug.Log( "Json not set..." );
				yield break;
			}	

			UnityWebRequest www = UnityWebRequest.Put( PutSessionURL, jsonStr );
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.SendWebRequest();

			Debug.Log( "Got this far...." + www.downloadHandler.text );

			//Move Uploaded File
		}

		//Upload Current Session File to Server
		// public void UploadFile()
		// {
			
		// 	//Search  directory for files
		// 	DirectoryInfo dir = new DirectoryInfo( path );
		// 	FileInfo[] info = dir.GetFiles( "*.dat" );

		// 	//Find all .dat files in the upload directory
		// 	foreach( FileInfo f in info )
		// 	{

		// 		Debug.Log( f.Name );
		// 		//Load the file 
		// 		System.Object obj = PersistenceManager.Instance.Load( f.Name );
				
		// 		//Convert the file to JSON
		// 		jsonString = JsonConvert.SerializeObject( obj );

		// 		//jsonString = JsonUtility.ToJson( obj );

		// 		//Display the file
		// 		Debug.Log( jsonString );

		// 		//Upload the file
		// 		var res =  StartCoroutine( PUT() );

		// 		Debug.Log( res );
				

		// 		//TODO

		// 		//Move Uploaded Files to sent directory
		// 		//Only if they were successfully uploaded..
		// 		File.Move( path + f.Name, destinationPath + f.Name );
		// 	}			
		// }

		//Send Put Request to the web server
		//Send the session data as a json string.
		private IEnumerator PUT()
		{
		
			Debug.Log( jsonString );

			UnityWebRequest www = UnityWebRequest.Put( PutSessionURL, jsonString );
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.SendWebRequest();

			Debug.Log( "Got this far...." + www.downloadHandler.text );

		}



		IEnumerator WaitForRequest( WWW data )
		{
			Debug.Log( "Uploading Json...." );
			yield return data;

			Debug.Log( "Got this far ..." );

			if( data.error != null )
			{
				Debug.Log( data.error );
			}
			else
			{
				Debug.Log( "WWW Request : " + data.text );
			}
		}

			//Return a valid filepath for various devices...
		private static string GetPath()
		{

			#if UNITY_EDITOR
				return Application.dataPath + "/";
			#elif UNITY_ANDROID
				return Application.persistentDataPath;
			#elif UNITY_IPHONE
				return Application.persistentDataPath + "/";
			#else
				return Application.dataPath + "/";
			#endif
		}

		private void CheckDirectoryExists( string path )
		{
				//check if directory doesn't exit
			if(!Directory.Exists(path))
			{    
				//if it doesn't, create it
				Debug.Log("Directory Path does not exist. So im creating it for you."); 
				Directory.CreateDirectory(path);
			}
			else
			{
				Debug.Log( "Directory exists . We are good to go :)" );
			}
		}
	}
}
