using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StarManager : MonoBehaviour 
{
	
	public static StarManager instance = null;
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
	
	void Awake()
	{
		if( instance == null )
		{
			instance = this;
		}
		else if( instance != this )
		{
			Destroy( gameObject );
		}

		//DontDestroyOnLoad( gameObject );
	}

	IEnumerator Start()
	{
		previousStar = -1;

		previousStarObject = firstStar;

		while( true )
		{
			isLetter = ( Random.value < 0.5f );

			for( int x = 0; x < starCount; x++ )
			{
				//AddStarsToPool( 1 );
				yield return null;
				CreateStarFromPool();
			}
			yield return new WaitForSeconds( 1.0f );

			Messenger.Broadcast( "ProximityCheck" );

			yield return new WaitForSeconds( delay );	

			Messenger.Broadcast( "Disable" );
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
		}
		else
		{
			star.gameObject.name = "Star_" + count; 
			star.NumberText.text = count.ToString();
		}	

		star.transform.parent = transform;
	
		star.gameObject.SetActive( true );
		
		count ++;
		
		if( count > 25 )
		{
			count = 1;
		}
	}
}
