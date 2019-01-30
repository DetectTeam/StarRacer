using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Newtonsoft.Json; //JSON NET Plugin

namespace StarRacer
{
	public class SessionManager : MonoBehaviour
	{
		private static SessionManager _instance;
   		public static SessionManager Instance { get { return _instance; } }

		private PlayerSelection playerSelection;

		private Session session;
		
		public Session CurrentSession { get{ return session; } }

		//[SerializeField] private string deviceName;
		[SerializeField] private string deviceModel;
		[SerializeField] private string deviceName;
		[SerializeField] private string deviceType;
		[SerializeField] private string deviceUniqueIdentifier;

		[SerializeField] private string sessionDuration;
		public string SessionDuration { get{ return sessionDuration; } set{ sessionDuration = value; }  }

		[SerializeField] private List<string> levelLayout = new List<string>();
		public List<string> LevelLayout { get{ return levelLayout; } set{ levelLayout = value; } }

		[SerializeField] private List<Star> currentLevelStars = new List<Star>();
		public List<Star> CurrentLevelStars { get{ return currentLevelStars; } set{ currentLevelStars = value; } }

		[SerializeField] private int levelLayoutCount = 0;

		public int LevelLayoutCount { get{ return levelLayoutCount; } set{ levelLayoutCount = value; } }

		public void Awake()
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

		[SerializeField] private string timeToStartSession;

		public string TimeToStartSession { get{ return timeToStartSession; } set{ 
			timeToStartSession = value; 
			if( timeToStartSession.Length > 0 )
			{
				session.TimeToStartSession = timeToStartSession;
			}	
		} }

		[SerializeField] private string transitionDuration;
		public string TransitionDuration { get{ return transitionDuration; } set{ transitionDuration = value; }  }

		[SerializeField] private string sessionUid;

		void Start()
		{	
			Session session = new Session();
			//deviceName = SystemInfo.deviceName;
			session.DeviceModel = SystemInfo.deviceModel;
			session.DeviceType = SystemInfo.deviceType.ToString();
			session.DeviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
			session.DeviceName = SystemInfo.deviceName;	
		}
		
		//Create a new Session
		//
		public void CreateSession()
		{
			Debug.Log( "Creating new Session...." );
			
			sessionUid = System.Guid.NewGuid().ToString();

			session =  new Session();
			session.SessionID = sessionUid;

			session.DeviceType = deviceType;
			session.DeviceModel = deviceModel;
			session.DeviceName = deviceName;
			session.DeviceUniqueIdentifier = deviceUniqueIdentifier;
			
			session.PlayerID = deviceUniqueIdentifier;
			session.Date =  string.Format("{0:dd-MM-yyyy}", System.DateTime.Now ); 
			session.TimeStamp = System.DateTime.Now.ToString();
			
			session.SessionDuration = sessionDuration;
			session.SessionName = "trail_maker_session_" +  sessionUid;
			session.SessionNumber = 22;
			
			PersistenceManager.Instance.FileName = "test.dat";
			session.FileName = session.SessionName + ".dat";
		}

		public void SetTargetStar( )
		{
			playerSelection.Target_Response_ID = LevelLayout[ levelLayoutCount ];
			SetTargetResponseLocation();
		}

		public void SetTargetResponseLocation( )
		{
			playerSelection.Target_Response_Location_X = ( float ) Math.Round( currentLevelStars[ levelLayoutCount ].gameObject.transform.position.x, 2 );
			playerSelection.Target_Response_Location_Y = ( float ) Math.Round( currentLevelStars[ levelLayoutCount ].gameObject.transform.position.y, 2 );
		}

		public void SetResponseLocation( float x, float y )
		{
			playerSelection.Response_Location_X = ( float ) Math.Round( x, 2 );
			playerSelection.Response_Location_Y = ( float ) Math.Round( y, 2 );
		}

		[SerializeField] private GameObject responseGameObject;
		public void SetResponseGameObject( GameObject obj )
		{
			responseGameObject = obj;
		}

		public void DistanceFromTarget( )
		{
			Debug.Log( "Level Layout Count: " +  levelLayoutCount );
			
			playerSelection.Distance_From_Target = ( float ) Math.Round( Vector2.Distance( responseGameObject.transform.position, currentLevelStars[ levelLayoutCount ].gameObject.transform.position ), 2 );
			Debug.Log( "Distance : " + playerSelection.Distance_From_Target );
		}

		public void CreateSelection()
		{
			Debug.Log( "Creating new Selection" );
			playerSelection = new PlayerSelection();	
		}

		public void EndSelection()
		{
			Debug.Log( "Selection Ending" );
			session.playerSelection.Add( playerSelection );
		}

		public void SetStartTime(  )
		{
			session.Absolute_Level_Start_Time = string.Format( "{0:hh-mm-ss}" , System.DateTime.Now );
		}

		private bool isFirstClick = false;
		public void SetRelativeTimeOfResponse(  )
		{
			// if( !isFirstClick )
			// { 
			// 	playerSelection.Relative_Time_Of_Response = string.Format( "{0:hh:mm:ss}" , System.DateTime.Now );
			// 	isFirstClick = true;
			// }
		}

		
		private float tmpTime; 
		public void CalculateRelativeTime( float timeElapsed )
		{
			tmpTime = tmpTime + timeElapsed;
			playerSelection.Relative_Time_Of_Response = ( float ) Math.Round (  tmpTime * 1000, 0);
			playerSelection.RT = ( float ) Math.Round ( timeElapsed * 1000, 0 );
		}

		public void SetHardCodedOrRandomized( int hardCodedOrRandomized )
		{
		 	if( hardCodedOrRandomized == 1 )
			 	session.Hard_Coded_Or_Randomized  = "Random";
			else
				session.Hard_Coded_Or_Randomized = "Hard";
		}

		// public void SetInterrupt( int interrupt )
		// {
		// 	session.Interrupt = interrupt;
		// }

		private int levelCount;
		public void SetLevel(  )
		{
			levelCount ++;
			session.Level = levelCount;
		}

		// public void SetTargetResponseId( int targetResponseId )
		// {
		// 	session.Target_Response_ID = targetResponseId;
		// }

		// public void SetTargetDistractor( int targetDistractor )
		// {
		// 	session.Target_Distractor = targetDistractor;
		// }

		 public void SetResponse(string response )
		{
			playerSelection.Response = response;
	 	}

		public void SetCorrect( int correct )
		{
			playerSelection.Correct = correct;
		}

		// public void SetProximityError( int proximityError )
		// {
		// 	session.ProximityError = proximityError;
		// }

		// public void SetPerservativeError( int perservativeError )
		// {
		// 	session.PreservativeError = perservativeError;
		// }

	
		public void EndSession()
		{
			CleanUp();
			Debug.Log( "Ending Session" );
			//Get the duration of the session
			//session.SessionDuration = sessionDuration;
			//Debug.Log( session );

			isFirstClick = false;
			tmpTime = 0;
			
			//Save the session
			PersistenceManager.Instance.Save( session );
		}
	
		public void SessionCompleted( bool b )
		{
			session.SessionCompleted = b;
		}

		private void CleanUp()
		{
			levelLayoutCount = 0;
			levelLayout.Clear();	
		}
	}
}
