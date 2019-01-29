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

		//[SerializeField] private string deviceName;
		[SerializeField] private string deviceModel;

		[SerializeField] private string deviceName;
		[SerializeField] private string deviceType;
		[SerializeField] private string deviceUniqueIdentifier;

		private Session session;
		private PlayerSelection playerSelection;
		public Session CurrentSession { get{ return session; } }

		[SerializeField] private string sessionDuration;
		public string SessionDuration { get{ return sessionDuration; } set{ sessionDuration = value; }  }

		[SerializeField] private List<string> levelLayout = new List<string>();
		public List<string> LevelLayout { get{ return levelLayout; } set{ levelLayout = value; } }

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


		// public int GetTargetStar()
		// {

		// }

		public void SetTargetStar( )
		{
			playerSelection.Target_Response_ID = LevelLayout[ levelLayoutCount ];
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
			if( !isFirstClick )
			{ 
				playerSelection.Relative_Time_Of_Response = string.Format( "{0:hh-mm-ss}" , System.DateTime.Now );
				isFirstClick = true;
			}
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

		// public void SetResponse( int response )
		// {
		// 	session.Response = response;
		// }

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
			
			//Save the session
			PersistenceManager.Instance.Save( session );
		}
	
		public void SessionCompleted( bool b )
		{
			session.SessionCompleted = b;
		}

		private void CleanUp()
		{
			levelCount = 0;
			levelLayout.Clear();	
		}
	}
}
