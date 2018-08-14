using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Error
{
	public bool ProximityError { get; set; }
	public bool PreservativeError { get; set; }

	public bool NumberToLetterError { get; set; }

	public bool LetterToNumberError { get; set; }

	public bool NoError { get; set; }

	public string Source{ get; set; } //Transition source
	public string Destination{ get; set; } //Transition Destination
	public string ErrorTimeStamp{ get; set; } //Time of error
	
}
