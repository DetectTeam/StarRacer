using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StarRacer
{
	[System.Serializable]

	// public class StarInfo
	// {
	// 	public string Location_X { get; set; }
	// 	public string Location_Y { get; set; }
	// 	public string Colour { get; set; }
	// }

	public class PlayerSelection
	{
		public string Relative_Time_Of_Response { get; set; }
		public int Interrupt { get; set; }
		public string Target_Response_ID { get; set; }
		public int Target_Distractor { get; set; }
		public string Response { get; set; }
		public int Correct { get; set; }
		public int ProximityError { get; set; }
		public int PreservativeError { get; set; }
		public string RT { get; set; }
		public string Target_Response_Location_X { get; set; }
		public string Target_Response_Location_Y { get; set; }
		public string Response_Location_X { get; set; }
		public string Response_Location_Y { get; set; }
		public string Distance_From_Target { get; set; }
		public string Location_X { get; set; }
		public string Location_Y { get; set; }
		public string Colour { get; set; }
	}

	[System.Serializable]
	public class Session
	{
		public string PlayerID { get; set; } //Which player?
		public string Date{ get; set; }
		public string Absolute_Level_Start_Time { get; set; }
		public string SessionID { get; set; }
		public string Hard_Coded_Or_Randomized { get; set; }
		public int Level { get; set; }
		//public List<StarInfo> starsInfo = new List<StarInfo>();
		public string SessionName { get; set; } // Do i need this ? Yes i do :) format: trail_maker_session_uid
		public int SessionNumber { get; set; } //unique number
		public string DeviceName { get; set; } //Device Name
		public string DeviceModel { get; set; } //Device Model
		public string DeviceType { get; set; } //Device Type
		public string DeviceUniqueIdentifier { get; set; } //Device Unique Identifier
		public string SessionDuration { get; set; } //Duration of session
		public string TimeStamp { get; set; } //Replace string with date time.
		public bool SessionCompleted { get; set; } //Did the player complete the session
		public string TimeToStartSession { get; set; } //Time taken for player to select first star after clicking start
		public int TransitionCount { get; set; }
		//public  List<Transition> transitions = new List<Transition>(); //Number of transitions that occured during a session
    	public string FileName { get; set; }
		public List<PlayerSelection> playerSelection = new List<PlayerSelection>(); 
	}
}
