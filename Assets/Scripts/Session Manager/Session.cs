using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameSessions
{

	[System.Serializable]
	public class Session
	{

		public string SessionName { get; set; } // Do i need this ? Yes i do :) format: trail_maker_session_uid
		public int SessionNumber { get; set; } //unique number
		public string PlayerID { get; set; } //Which player?
		public string GameID { get; set; } //which game ?
		public string DeviceName { get; set; } //Device Name
		public string DeviceModel { get; set; } //Device Model
		public string DeviceType { get; set; } //Device Type
		public string DeviceUniqueIdentifier { get; set; } //Device Unique Identifier
		public string SessionDuration { get; set; } //Duration of session
		public string TimeStamp { get; set; } //Replace string with date time.
		public bool SessionCompleted { get; set; } //Did the player complete the session

		public string TimeToStartSession { get; set; } //Time taken for player to select first star after clicking start

		public int TransitionCount { get; set; }

		public  List<Transition> transitions = new List<Transition>(); //Number of transitions that occured during a session
	
    	public string FileName { get; set; }
	
	
	}


}
