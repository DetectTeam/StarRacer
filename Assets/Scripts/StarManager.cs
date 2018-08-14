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

	void Start()
	{
		previousStar = -1;

		previousStarObject = firstStar;
		
	}

}
