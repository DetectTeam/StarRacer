using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour 
{

	public void ToggleSettings()
	{
		if( gameObject.activeSelf )
		{
			gameObject.SetActive( false );
		}
		else
		{
			gameObject.SetActive( true );
		}
	}
}
