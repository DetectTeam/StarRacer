using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Resonsible for managing the trail maker tests 
//As of this build there are 4 test or levels
//A level is a prefab that contains the assets and layout for a Trailmaker test
public class LevelHandler : MonoBehaviour 
{

	[SerializeField] private Constellation[] constellationList; //List of levels
	[SerializeField] private GameObject[] demoList; //List of demo levels
	[SerializeField] int currentLevel = 0;
	[SerializeField] bool demoMode;

	[SerializeField] bool enablePlayerPrefs;


	void Start()
	{
		
		if( !enablePlayerPrefs )
			PlayerPrefs.DeleteAll();
	
		//When the level is loaded
		//Check what value the current level has 0 - 3 
		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		{
			currentLevel = PlayerPrefs.GetInt( "CurrentLevel" );
			Debug.Log( currentLevel );
		}

		//Activate the current level
		SetActiveLevel();
		
		    
	}


	//Load the scene level
	public void LoadLevel()
	{
		SceneManager.LoadSceneAsync( "Star_Racer_Main" );
	}



	//Deactivate all levels
	//Activate the current level
	private void SetActiveLevel()
	{
		//Disable all level prefabs contained in the list
		foreach( Constellation c in constellationList )
		{
			c.constellation.SetActive( false );
		}

		if( !GameManager.Instance.IsDemoMode )
		{	//Enable the currently selected level
			constellationList[ currentLevel ].constellation.SetActive( true );
			Debug.Log( "Brodcasting ScoreLimit" );
			
			//Set the score limit for this level.
			//Broadcast is revieved by Gamemanager
			Messenger<int>.Broadcast( "ScoreLimit", constellationList[ currentLevel ].starCount );
		}
		else
		{
			demoList[0].SetActive( true );
		}
	}

	//Called by the next level buttons On click event 
	public void SetNextLevel(  )
	{
		if( currentLevel < constellationList.Length -1  )
			currentLevel ++;
		else
			currentLevel = 0;


		PlayerPrefs.SetInt( "CurrentLevel", currentLevel );		

		
	}


	
}
