﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



	public class DialogueManager : MonoBehaviour//Singleton<DialogueManager> 
	{
		public static DialogueManager Instance;
		
		//[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private TextMeshProUGUI dialogueText;
		[SerializeField] private Queue<string> sentences; 
		[SerializeField] private Animator animator;
		[SerializeField] private Button btnContinue;

		private bool isSectionComplete;

		public bool IsSectionComplete { get{ return isSectionComplete; } set{ isSectionComplete = value; } }

		private Coroutine typeSentence;

		private void Awake()
		{
			//Check if instance already exists
             if (Instance == null)
                //if not, set instance to this
                 Instance = this;
            //If instance already exists and it's not this:
             else if (Instance != this)
                 //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                 Destroy(gameObject);    
        //     //Sets this to not be destroyed when reloading scene
             //DontDestroyOnLoad(gameObject);
		}


		private void OnEnable()
		{
			Messenger.AddListener( "DisplayNextSentence" , DisplayNextSentence );
			//Messenger.MarkAsPermanent( "DisplayNextSentence" );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "DisplayNextSentence" , DisplayNextSentence );	
		}

		private void Start () 
		{
			sentences = new Queue<string>();
		}

		public void StartDialogue( Dialogue dialogue )
		{
			Debug.Log( "Starting Dialogue..." );
			//nameText.text = dialogue.name;

			sentences.Clear();

			foreach( string sentence in dialogue.sentences )
			{
				sentences.Enqueue( sentence );
			}

			DisplayNextSentence();
		}

		private int count = 0;
		public void DisplayNextSentence()
		{
			Debug.Log( "Display next sentence: " + sentences.Count );
			
			if( sentences.Count == 0 )
			{
				Debug.Log( "No More Sentences..." );
				EndDialogue();
				return;
			}

			string sentence = sentences.Dequeue();
			dialogueText.text = "";
			
			if( typeSentence != null ) 
				StopCoroutine( typeSentence );
			else
				Debug.Log( "Nothing to stop !!!!" );
			 
			 
			 typeSentence =  StartCoroutine( TypeSentence( sentence ) );
		}

		private IEnumerator TypeSentence( string sentence )
		{
			dialogueText.text = "";

			foreach( char letter in sentence.ToCharArray() )
			{
				dialogueText.text += letter;
				yield return null;
			}

			btnContinue.gameObject.SetActive( true );
		}

		private void EndDialogue()
		{
			Debug.Log( "Ending Dialogue" );
			//Messenger.Broadcast( "SectionOver" );
			isSectionComplete = true;
			dialogueText.text = "";
			//animator.SetBool( "IsOpen", false );
			btnContinue.gameObject.SetActive( false );
		}

		public void Reset()
		{
			if( sentences.Count > 0 )
			{
				sentences.Clear();
			}
		}
	}

