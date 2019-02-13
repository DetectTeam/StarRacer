using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarRacer
{
	public class NumberTutorial : MonoBehaviour 
	{

		[SerializeField] private GameObject dialogueBox;
		[SerializeField] private List<Dialogue> dialogues;

		[SerializeField] private List<Star> stars;

		[SerializeField] private Button buttonContinue;
		
		private MoveTo moveDialog;

		private void OnEnable()
		{
			buttonContinue.onClick.AddListener( ContinueButtonAction );
		}

		private void OnDisable()
		{
			buttonContinue.onClick.RemoveListener( ContinueButtonAction );
		}
		
		// Use this for initialization
		private void Start () 
		{
			moveDialog = dialogueBox.GetComponent<MoveTo>();
			
			StartCoroutine( Tutorial() );


		}
		
		private IEnumerator Tutorial()
		{
			
			DisableStarColliders();
			yield return new WaitForSeconds( 1.0f );
			StartDialog( 0 );

			//Wait for user to exhaust dialogue
			while (DialogueManager.Instance && !DialogueManager.Instance.IsSectionComplete)
			{
				yield return null;

				if (DialogueManager.Instance == null)
					break;
			}

			Debug.Log( "Start selecting the stars...." );

			for( int x = 0; x < 3; x++ )
			{
				yield return new WaitForSeconds( 0.5f );
				stars[x].TriggerCorrectSelection();
				
			}

			yield return new WaitForSeconds( 1.5f );

			StartDialog( 1 );

			//Wait for user to exhaust dialogue
			while (DialogueManager.Instance && !DialogueManager.Instance.IsSectionComplete)
			{
				yield return null;

				if (DialogueManager.Instance == null)
					break;
			}

			for( int i = 0; i < 2; i++ )
			{
				yield return new WaitForSeconds( 1.5f );
				stars[5].TriggerInCorrectSelection();
			}

			yield return new WaitForSeconds( 1.0f );

			ResetStarColours();

			EnableStarColliders();


			yield return new WaitForSeconds( 1.0f );

			//ToggleDialogBox(0.3f, new Vector3(0f, -350f, 0f), new Vector3(0f, -1500f, 0f));
		}

		private void DisableStarColliders()
		{
			foreach( Star s in stars )
				s.DisableCollider();
		}

		private void EnableStarColliders()
		{
			foreach( Star s in stars )
				s.EnableCollider();
		}
		
		private void  ResetStarColours()
		{
			foreach( Star s in stars )
				s.ResetColour();
		}

		private void StartDialog( int index )
		{
			Debug.Log( "Starting Dialogue..." );
			
			if( DialogueManager.Instance )
				DialogueManager.Instance.StartDialogue( dialogues[ index ] );
		}


		private int count = 0;
		private void ContinueButtonAction()
		{
			count++;

			Debug.Log( "Button count: " + count );

			if( count == 4 )
			{
				ToggleDialogBox( 0.3f, new Vector3( 0f, -350f, 0f ), new Vector3( 0f, -1500f, 0f ) );
				Messenger.Broadcast( "StartTimer" );
				SessionManager.Instance.StartTimer();
				SessionManager.Instance.SetStartTime( );
				SessionManager.Instance.SetNumberOrLetter( false );
			}
		}

		private void ToggleDialogBox( float speed, Vector3 source, Vector3 target )
    	{
        //Hide Dialogue Box
        if (moveDialog)
            moveDialog.Move( speed, source, target );
    	}		
	}
}
