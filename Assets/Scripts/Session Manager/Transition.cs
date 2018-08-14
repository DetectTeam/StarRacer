using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	

[System.Serializable]	
public class Transition
	{
	public int TransitionNo { get; set; }
	public string TransistionName { get; set; }

	public string TransitionTime{ get; set; }

	public string Source { get; set; }
	public string Destination{  get; set; }

	public bool TransitionComplete { get; set; }

	public int proximityErrorCount;
	public int preservativeErrorCount;
	public int numToLetterCount;
	public int letterToNumCount;

	public int ErrorCount { get; set; }


	//public List <Error> transitionErrors = new List<Error>(); 
}
