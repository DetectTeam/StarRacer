using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressStar : MonoBehaviour 
{

	private TextMeshPro txtInput;

	private void Awake()
	{
		txtInput = GetComponent<TextMeshPro>();

	
	}

	private void OnEnable()
	{
		Messenger<string>.AddListener( "UpdateProgress", SetTxtInput );
	}

	private void OnDisable()
	{
		Messenger<string>.RemoveListener( "UpdateProgress", SetTxtInput );
	}

	private void SetTxtInput( string txt )
	{
		if( txt.Length > 0 &&  txt.Length <= 2 )
			txtInput.text = txt;
		else
			Debug.Log( "Invalid Text" );
	}

	
	
}
