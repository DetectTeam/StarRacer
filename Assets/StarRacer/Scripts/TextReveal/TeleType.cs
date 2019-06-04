using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleType : MonoBehaviour {

	
	private TextMeshProUGUI blurbText;

	
	
	// Use this for initialization
	void Start () 
	{
		Time.timeScale = 1.0f;
		//Get reference to TextMeshPro component if it exists . Otherwise add one
		blurbText = GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();

		//StartCoroutine( RevealText() );
		blurbText.alpha = 0.0f;
		
	}


	public IEnumerator RevealText()
	{
		
		float tmpAlpha = 1.0f;

		yield return new WaitForSeconds( 0.25f );
		blurbText.alpha = 1.0f;
		
		
		//Get all visible characters in text object
		int totalVisibleCharacters = blurbText.textInfo.characterCount;
		
		int count = 0;

		while( true )
		{
			
		
			int visibleCount = count % ( totalVisibleCharacters + 1 );
			
			blurbText.maxVisibleCharacters = visibleCount;
			
			if( visibleCount >= totalVisibleCharacters )
			{
				
				break;
			}


			count ++;	

			
			yield return new WaitForSeconds( 0.05f );
		}

		//Fade Out text.

		yield return new WaitForSeconds( 2.5f );

		while( blurbText.alpha > 0 )
		{
			
			yield return new WaitForSeconds( 0.05f );

			tmpAlpha = tmpAlpha - 0.05f;
			blurbText.alpha = tmpAlpha;


		}
	}
	
	
}
