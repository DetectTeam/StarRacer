using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarRacer
{
	public class StarManager : MonoBehaviour 
	{		
		public static StarManager Instance = null;
		public Star firstStar;
		public static int score = 0;
		public static int previousStar;
		public static Star previousStarObject;
		//public int PreviousStar { get{ return previousStar; } set{ previousStar = value; Debug.Log( previousStar ); } }
		//Records the number of points of the line the last time the player selected the correct number
		public static int lastSuccessPointCount = 0; 
		[SerializeField] private int delay;
		[SerializeField] private int starCount;
		[SerializeField] private float vertExtent;
		[SerializeField] private float horzExtent;
		[SerializeField] private bool isLetter;
		[SerializeField] private string[] letterLevel;
		[SerializeField] private bool isFirstRun = true;

		[SerializeField] private GameObject lastStarSelected;
		public GameObject LastStarSelected { get{ return lastStarSelected; } set{ lastStarSelected = value; } }
		
		[SerializeField] private float timeElapsedBetweenPresses = 0;
		public float TimeElapsedBetweenPresses { get{ return timeElapsedBetweenPresses; } set{ timeElapsedBetweenPresses = value; } }

		[SerializeField] private bool isButtonPressed = true;
		public bool IsButtonPressed { get{ return isButtonPressed; } set{ isButtonPressed = value; } }
 

		void Awake()
		{
			if( Instance == null )
			{
				Instance = this;
			}
			else if( Instance != this )
			{
				Destroy( gameObject );
			}

			//DontDestroyOnLoad( gameObject );
		}

		private void Start()
		{
			//previousStar = -1;

			//previousStarObject = firstStar;
			//LoadRandomLevel();	
		}


		private void Update()
		{
			if( isButtonPressed )
			{
				timeElapsedBetweenPresses = timeElapsedBetweenPresses + Time.deltaTime;
				//Debug.Log( timeElapsedBetweenPresses );
			}
		}


		public void  DisableStars()
		{
			Messenger.Broadcast( "Disable" );
		}

		public void LoadRandomLevel() 
		{ 
			StartCoroutine( "IELoadRandomLevel" );
		}

		private IEnumerator IELoadRandomLevel()
		{
			// if( !isFirstRun  )
			// 	Messenger.Broadcast( "Disable" );

			//yield return new WaitForSeconds( 1.0f );

			//Create new session	
			SessionManager.Instance.SetStartTime( );
			SessionManager.Instance.SetHardCodedOrRandomized( 1 );
			//SessionManager.Instance.SetLevel( );
			
			isLetter = ( Random.value < 0.5f );

			for( int x = 0; x < starCount; x++ )
			{
				//AddStarsToPool( 1 );
				yield return null;
				CreateStarFromPool();
			}
			yield return new WaitForSeconds( 1.0f );

			Messenger.Broadcast( "ProximityCheck" );

			isFirstRun = false;
		}

		private void AddStarsToPool( int starCount )
		{
			for( int x = 0; x < starCount; x++ )
			{
				var star = StarPool.Instance.Get();
				star.transform.parent = transform;	
				star.gameObject.name = "Star_" + x;
			}
		}

		private static int count = 1;
		
		private void CreateStarFromPool()
		{
			var star = StarPool.Instance.Get();

			if( count == 1 )
				star.IsCorrect = true;

			star.OrderIndex = count;	
			
			if( isLetter )
			{
				star.gameObject.name = "Star_" + letterLevel[ count-1 ]; 
				star.NumberText.text = letterLevel[ count-1 ].ToString();
				SessionManager.Instance.LevelLayout.Add( letterLevel[ count-1 ].ToString() );
			}
			else
			{
				star.gameObject.name = "Star_" + count; 
				star.NumberText.text = count.ToString();
				SessionManager.Instance.LevelLayout.Add( count.ToString()  );
			}	

			star.transform.parent = transform;
		
			star.gameObject.SetActive( true );

			SessionManager.Instance.CurrentLevelStars.Add( star );
			
			
			count ++;
			
			if( count > 25 )
			{
				count = 1;
			}
		}
	}
}
