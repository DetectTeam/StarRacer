using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour 
{
	
	[SerializeField] private bool isTimerRunning = false;
	[SerializeField] private float startTime;

	[SerializeField] private StringBuilder timeTaken;
	[SerializeField] private StringBuilder timeTakenSeconds;

	[SerializeField] private TextMeshProUGUI timerText;

	public StringBuilder TimeTaken { get { return timeTaken; } }
	public StringBuilder TimeTakenSeconds{ get { return timeTakenSeconds; } }


	private float timeTakenfloat;
	public float TimeTakenfloat{ get { return timeTakenfloat; } }

	private float t;

	private SessionManager sessionManager;


	private StringBuilder minutes;
	private StringBuilder seconds;

	
	// Use this for initialization
	void Start () 
	{
		startTime = Time.time;
		GameObject gameManager = GameObject.Find( "GameManager" );

		if( gameManager != null )
			sessionManager = gameManager.GetComponent<SessionManager>();

		timerText = GetComponent<TextMeshProUGUI>();

		minutes = new StringBuilder("");	
		seconds = new StringBuilder("");
		timeTaken = new StringBuilder("");
		timeTakenSeconds = new StringBuilder("");
		
	}


	
	// Update is called once per frame
	void Update () 
	{
		
		if( isTimerRunning )
		{
			
			minutes.Clear();
			seconds.Clear();
			timeTaken.Clear();
			timeTakenSeconds.Clear();
			
			timeTakenfloat = t = Time.time - startTime;

			//string minutes = "";
			//string seconds = "";
			
			if( ((int)t / 60 ) < 10 )
			{
				minutes.Append( "0" + ( (int) t / 60 ).ToString(  ) ) ;
			}
			else
			{
				minutes.Append( ( (int) t / 60 ).ToString( "F2" ) );
			}


			if( ( t % 60 ) < 10 )
			{
				seconds.Append( "0" + ( (int) t % 60 ).ToString(  ) );
				
			}
			else
			{
				seconds.Append( ((int)t % 60).ToString(  ) );
			}

			timerText.text = minutes + ":" + seconds;

			
			timeTaken.Append( minutes + ":" + seconds );


			timeTakenSeconds.Append( minutes + ":" + ( t % 60 ).ToString( "F2" ));

	
		}

		
	}

	

	
	//Triggered when game starts
	public void StartTimer() 
	{
		isTimerRunning = true;
	}

	//Triggered when player completes a level
	public void StopTimer()
	{
		Debug.Log( "Time Taken : " +  timeTaken );
		sessionManager.SessionDuration = timeTaken.ToString();
		Debug.Log( "Time Taken  Session: " +  sessionManager.SessionDuration );
		isTimerRunning  = false;
		

	}
}
