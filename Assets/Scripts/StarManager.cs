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

		[SerializeField] private List<Color> colourList;

		[SerializeField] private Color grey;
 
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
            //Create new session	
            SessionManager.Instance.SetStartTime();

            SessionManager.Instance.SetHardCodedOrRandomized(1);
            //SessionManager.Instance.SetLevel( );
            SetLevelCount();
            SetLevelType();

            //isLetter = ( Random.value < 0.5f );

            SessionManager.Instance.SetNumberOrLetter(isLetter);

            //colourList.ShuffleList();

            for (int x = 0; x < starCount; x++)
            {
                //AddStarsToPool( 1 );
                yield return null;
                CreateStarFromPool();
            }
            yield return new WaitForSeconds(1.0f);

            Messenger.Broadcast("ProximityCheck");

            isFirstRun = false;
        }

        private int levelCount = 5;

		private void SetLevelCount()
		{
			if( PlayerPrefs.HasKey( "LevelCount" ) )
			{
				levelCount = PlayerPrefs.GetInt( "LevelCount" );
			}
			else
			{
				PlayerPrefs.SetInt( "LevelCount", levelCount );
			}
			
			SessionManager.Instance.SetLevel( levelCount );

			//PlayerPrefs.SetInt( "LevelCount", levelCount );
		}

        private void SetLevelType()
        {
            if (PlayerPrefs.HasKey("LevelType"))
            {
                int levelType = PlayerPrefs.GetInt("LevelType");

                if (levelType == 1)
                {
                    isLetter = true;
                    PlayerPrefs.SetInt("LevelType", 0);
					SessionManager.Instance.IsLetterLevel = true;
                }
                else
                {
                    isLetter = false;
                    PlayerPrefs.SetInt("LevelType", 1);
					SessionManager.Instance.IsLetterLevel = false;
                }
            }
            else
            {
                isLetter = false;
                PlayerPrefs.SetInt("LevelType", 1);
            }
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
				
				if( (count - 1) == 0 )
				{
					star.Distractor = "NA";
				}
				else if( count < letterLevel.Length )
				{
					star.Distractor = letterLevel[ count ].ToString();
				}
				else if( count  == letterLevel.Length )
				{
					star.Distractor = "NA";
				}
				
				star.StarName = letterLevel[ count -1 ].ToString();
				star.IsLetter = true;
				SessionManager.Instance.LevelLayout.Add( letterLevel[ count-1 ].ToString() );

			}
			else
			{
				star.gameObject.name = "Star_" + count; 
				star.NumberText.text = count.ToString();
				star.StarName = letterLevel[ count -1 ].ToString();
				star.Distractor = "NA";
				star.IsLetter = false;
				SessionManager.Instance.LevelLayout.Add( count.ToString()  );
			}	

			star.StarSpriteRenderer.color = grey;//colourList[ count - 1 ];
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
