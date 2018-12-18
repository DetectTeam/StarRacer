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
	[SerializeField] private int starCount;
	[SerializeField] private float vertExtent;
	[SerializeField] private float horzExtent;

	[SerializeField] private bool isLetter;

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
			for( int x = 0; x < starCount; x++ )
			{
				//AddStarsToPool( 1 );
				yield return new WaitForSeconds( 0.1f );
				CreateStarFromPool();
			}

			yield return new WaitForSeconds( 500.0f );	

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
		
		star.gameObject.name = "Star_" + count; 
		star.transform.parent = transform;
		star.NumberText.text = count.ToString();
		star.gameObject.SetActive( true );
		
		count ++;
		
		if( count > 23 )
			count = 1;
	}


}
