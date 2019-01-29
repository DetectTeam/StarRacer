using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StarRacer
{
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

		private void OnEnable()
		{
			Messenger.AddListener( "AddTime", AddToTime );
			Messenger.AddListener( "SubtractTime", SubtractFromTime );
		}

		private void  OnDisable()
		{
			Messenger.RemoveListener( "AddTime", AddToTime );
			Messenger.RemoveListener( "SubtractTime", SubtractFromTime );
		}

		
		// Use this for initialization
		void Start () 
		{
			startTime = Time.time;
			GameObject gameManager = GameObject.Find( "GameManager" );

			if( gameManager != null )
				sessionManager = gameManager.GetComponent<SessionManager>();

			timerText = GetComponent<TextMeshProUGUI>();
			timerText.enabled = false;

			minutes = new StringBuilder("");	
			seconds = new StringBuilder("");
			timeTaken = new StringBuilder("");
			timeTakenSeconds = new StringBuilder("");
		
		}

		[SerializeField] private float timeLeft;
		private IEnumerator CountDown()
		{
			float count = 0;
			timerText.enabled = true;
			DisplayTime();

			yield return new WaitForSeconds( 1.0f );

			while( timeLeft > 0 )
			{
				yield return new WaitForSeconds( 0.1f );
				count ++;
				if( count == 10 )
				{
					timeLeft --;
					DisplayTime();
					count = 0;
				}	
			}

			Debug.Log( "Game Over" );
		}
		
		private void DisplayTime()
		{
				minutes.Clear();
				seconds.Clear();
				timeTaken.Clear();
				timeTakenSeconds.Clear();
				
				t = timeLeft;


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
		
		//Triggered when game starts
		public void StartTimer() 
		{
			//isTimerRunning = true;
			StartCoroutine( "CountDown" );
		}

		//Triggered when player completes a level
		public void StopTimer()
		{
			StopCoroutine( "CountDown" );
			Debug.Log( "Time Taken : " +  timeTaken );
			sessionManager.SessionDuration = timeTaken.ToString();
			Debug.Log( "Time Taken  Session: " +  sessionManager.SessionDuration );
			isTimerRunning  = false;
		}

		public void AddToTime()
		{
			timeLeft = timeLeft + 3.0f;
			DisplayTime();
			PunchScale( timerText.gameObject );
		}

		public void SubtractFromTime()
		{
			timeLeft = timeLeft - 3.0f;

			if( timeLeft < 0 )
				timeLeft = 0;
				
			DisplayTime();
			ColourFade( timerText.gameObject, timerText.color );
		}

		private void PunchScale( GameObject obj )
		{
			iTween.PunchScale( obj, iTween.Hash( "x", +0.5f, "y", +0.5f, "time", 1f ) );
		}

		[SerializeField] private Color errorColour;
		public void ColourFade( GameObject obj, Color color  )
		{
		
			iTween.ValueTo ( obj, iTween.Hash (
						"from", errorColour, 
						"to", color, 
						"time", 1.25f, 
						"easetype", "easeInCubic", 
						"onUpdate","UpdateColor"));
		}

		private void UpdateColor( Color newColor )
		{
			timerText.color = newColor;
		}
	}
}
