using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextRevealer : MonoBehaviour 
{

	[SerializeField] private Text text;
	[SerializeField] TextMeshProUGUI txtPro;
	[SerializeField] float  textSpeed = 0.07f;

	void Start()
	{
		Time.timeScale = 1.0f;
		//StartCoroutine( RevealText() );
		
	}

	IEnumerator RevealText()
	{

		var originalString = text.text;
		text.text = "";

		int numCharsRevealed = 0;
		while( numCharsRevealed < originalString.Length )
		{

			while( originalString[ numCharsRevealed ] == ' ' )
				++numCharsRevealed;


			++numCharsRevealed;
			text.text = originalString.Substring(0, numCharsRevealed);

			yield return new WaitForSeconds( textSpeed );
		

		}

	}

}
