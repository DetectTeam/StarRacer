using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StarRacer
{
	public class DialogueTrigger : MonoBehaviour 
	{
		public Dialogue dialogue;

		public void TriggerDialogue()
		{
			DialogueManager.Instance.StartDialogue( dialogue );
		}
	}
}
