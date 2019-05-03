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

		private Session session;
		private PlayerSelection playerSelection;
		private StarInfo starInfo;
		
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

		[SerializeField] private string transitionDuration;
		public string TransitionDuration { get{ return transitionDuration; } set{ transitionDuration = value; }  }

		[SerializeField] private string sessionUid;

		[SerializeField] private GameObject responseGameObject;

		public bool IsLetterLevel { get; set; }

		private Coroutine timer;

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

		void Start()
		{	
			//session = new Session();
			//deviceName = SystemInfo.deviceName;
			//session.DeviceModel = SystemInfo.deviceModel;
			//session.DeviceType = SystemInfo.deviceType.ToString();
			//session.DeviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
			//session.DeviceName = SystemInfo.deviceName;	
		}
		
		//Create a new Session
		//
		public void CreateSession()
		{
			Debug.Log( "Creating new Session...." );

			//sessionUid = System.Guid.NewGuid().ToString();
			

			session = new Session();
			
			
			session.DeviceType = deviceType;
			session.DeviceModel = deviceModel;
			session.DeviceName = deviceName;
			session.DeviceUniqueIdentifier = deviceUniqueIdentifier;
			
			session.PlayerID = SystemInfo.deviceUniqueIdentifier;
			session.Date =  string.Format("{0:dd-MM-yyyy}", System.DateTime.Now ); 
			session.TimeStamp = System.DateTime.Now.ToString();
			
			session.SessionDuration = sessionDuration;
			session.SessionName = "trail_maker_session_" +  sessionUid;
			session.SessionNumber = sessionUid;
			
			PersistenceManager.Instance.FileName = "test.dat";
			session.FileName = session.SessionName + ".dat";

			session.Hard_Coded_Or_Randomized = "Hard Coded";
		}

		public void SetTargetStar(  )
		{
			playerSelection.Target_Response_ID = LevelLayout[ levelLayoutCount ];
			
			if( !IsLetterLevel )
			{
				playerSelection.Target_Distractor =  "NA";
			}
			else if( levelLayoutCount == 0 )
			{
				Debug.Log( "LEvel layout counter = 0");
				playerSelection.Target_Distractor =  "NA";
			}
			else if( levelLayoutCount == ( LevelLayout.Count - 1 ) )
			{
				Debug.Log( levelLayoutCount + " = " + ( LevelLayout.Count - 1 ) );
				playerSelection.Target_Distractor =  "NA";
			}
			else if( levelLayoutCount < LevelLayout.Count - 1 )
				playerSelection.Target_Distractor = LevelLayout[ levelLayoutCount + 1 ];
		
			SetTargetResponseLocation();
		}

		public void SetTargetResponseLocation( )
		{
			playerSelection.Target_Response_Location_X = ( float ) Math.Round( currentLevelStars[ levelLayoutCount ].gameObject.transform.position.x, 2 );
			playerSelection.Target_Response_Location_Y = ( float ) Math.Round( currentLevelStars[ levelLayoutCount ].gameObject.transform.position.y, 2 );

			//Debug.Log( "Current Selection : " + currentLevelStars[ levelLayoutCount ].gameObject.name );
		}

		public void SetResponseLocation( float x, float y )
		{
			playerSelection.Response_Location_X = ( float ) Math.Round( x, 2 );
			playerSelection.Response_Location_Y = ( float ) Math.Round( y, 2 );
		}

		public void SetResponseGameObject( GameObject obj )
		{
			responseGameObject = obj;

			//Debug.Log( "Current Selection : " + obj.name );
		}

		public void CheckForProximityError()
		{
			int count = 0;
			var currentTargetStar = currentLevelStars[ levelLayoutCount ].GetComponent<Star>();
			var selectedStar = responseGameObject.GetComponent<Star>();

			Debug.Log( "Current Proximity List : " + currentTargetStar.ProximityStars.Count );

			if( currentTargetStar.ProximityStars.Count >= 6 )
				count = 6;
			else
				count = currentTargetStar.ProximityStars.Count;

			for( int i = 1; i < count; i++  )
			{
                if( selectedStar.Uid == currentTargetStar.ProximityStars[i].GetComponent<Star>().Uid )
				{
					playerSelection.ProximityError = 1;
					Debug.Log( "Proximity Error" );
				}
			}
		}

		public void DistanceFromTarget( )
		{
			//Debug.Log( "Level Layout Count: " +  levelLayoutCount );
			
			playerSelection.Distance_From_Target = ( float ) Math.Round( Vector2.Distance( responseGameObject.transform.position, currentLevelStars[ levelLayoutCount ].gameObject.transform.position ), 2 );
			//Debug.Log( "Distance : " + playerSelection.Distance_From_Target );
		}

		public void CreateSelection()
		{
			//Debug.Log( "Creating new Selection" );
			playerSelection = new PlayerSelection();	
		}

		public void EndSelection()
		{
			
			//playerSelection.Target_Distractor = currentTargetDistractor.Trim();
			//Debug.Log( "Selection Ending" );
			session.PlayerSelection.Add( playerSelection );
		}

		public void SetStartTime(  )
		{
			session.Absolute_Level_Start_Time = string.Format( "{0:hh-mm-ss}" , System.DateTime.Now );
		}

		private float tmpTime; 
		public void CalculateRelativeTime( float timeElapsed )
		{
			tmpTime += timeElapsed;
			playerSelection.Relative_Time_Of_Response += (float) Math.Round( ( tmpTime * 1000 ), 2);	
		}

		public void CalculateRT()
		{
			playerSelection.RT = (float) Math.Round( ( timeElapsed * 1000 ), 2 );
			
			tmpTime += playerSelection.RT;

			playerSelection.Relative_Time_Of_Response = tmpTime;

			timeElapsed = 0;
		}

		public void SetHardCodedOrRandomized( int hardCodedOrRandomized )
		{
		 	if( hardCodedOrRandomized == 1 )
			 	session.Hard_Coded_Or_Randomized  = "Random";
			else
				session.Hard_Coded_Or_Randomized = "Hard";
		}

		public void SetNumberOrLetter( bool numOrLetter )
		{
			if( numOrLetter )
				session.Number_Letter = "Number_Letter";
			else
				session.Number_Letter = "Number";
		}

		private int levelCount;
		public void SetLevel( int level )
		{
			session.Level = level;
		}

		public void SetResponse(string response )
		{
			playerSelection.Response = response.Trim();
	 	}

		public void SetCorrect( int correct )
		{
			playerSelection.Correct = correct;
		}

		private string currentTargetDistractor;
		public void SetTargetDistractor( string targetDistractor )
		{
			currentTargetDistractor = targetDistractor.Trim();
		}


		//Seperate into seperate class
		[SerializeField] private Star previousStar;
		public Star PreviousStar { get{ return previousStar; } set{ previousStar = value; } }

		[SerializeField] private Star currentStar;

		public Star CurrentStar{ get{ return currentStar; } set{ currentStar = value; } }

		public void CheckForPreservativeError()
		{
			if( previousStar != null )
			{
				//Debug.Log( "PREVIOUS STAR VALUE: " + previousStar.StarValue );
				//Debug.Log( "Current Star Value: " + currentStar.StarValue.Length );
				
				//Get Current star
				char previousStarC = previousStar.StarValue[0];
				char currentStarC = currentStar.StarValue[0];

				var nextChar = (char)(((int) previousStarC ) + 1);

				if( nextChar == currentStarC )
				{
					Debug.Log( "PRESERVATIVE ERROR" );
					playerSelection.PreservativeError =  1;
				}
				else
				{
					Debug.Log( "NO PRESERVATIVE ERROR" );
					playerSelection.PreservativeError =  0;
				}
			}
		}

		//Seperate ends

		public void EndSession()
		{
			CleanUp();
			Debug.Log( "Ending Session" );

			tmpTime = 0;
			Debug.Log( "End Session ::::: :" + PlayerPrefs.GetString( "user_id" ) );
			session.SessionID = PlayerPrefs.GetString( "user_id" );

			//Save the session
			PersistenceManager.Instance.Save( session );
			
			string jsonString = JsonConvert.SerializeObject( session );

			FileUploadHandler.Instance.PUT( jsonString );
			
			SessionCompleted( true );
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

		public void CreateNewStarInfoList()
		{
			starInfo = new StarInfo();
		}

		public void SetStarInfo( Star star )
		{
			//var star = starObj.GetComponent<Star>();
			StarInfo starInfo = new StarInfo();

			starInfo.StarName = star.StarName;
			starInfo.Location_X = (float) Math.Round( star.StarPositionX , 2 );
			starInfo.Location_Y = (float) Math.Round( star.StarPositionY , 2 );
			starInfo.ColourCode = star.Colourcode;

			session.StarInfo.Add( starInfo );
		}

		public void SetStarInfo( float x, float y, int colourCode )
		{
			StarInfo starInfo = new StarInfo();
			
			starInfo.Location_X = (float) Math.Round( x );
			starInfo.Location_Y = (float) Math.Round( y );
			starInfo.ColourCode = colourCode;

			session.StarInfo.Add( starInfo );
		}

		public void StartTimer()
		{
			timer = StartCoroutine( RecordTime() );
		}

		public void StopTimer()
		{
			if( timer != null )
				StopCoroutine( timer );
		}

		[SerializeField] private float timeElapsed;
		private IEnumerator RecordTime ( )
		{
			while( true )
			{
				yield return new WaitForSeconds( 0.1f );
				timeElapsed += 0.1f;
			}
		}

		private void OnApplicationQuit()
		{
			//levelCount = 0;
			//PlayerPrefs.SetInt( "LevelCount" , levelCount );
		}
	}
}
