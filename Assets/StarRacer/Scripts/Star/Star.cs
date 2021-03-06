﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace StarRacer
{
	public class Star : MonoBehaviour 
	{
		
		[SerializeField] private bool isTutorial;
		[SerializeField] private bool isRandom;
		[SerializeField] private bool isLetter;
		
		private Transform _transform;

		public bool IsLetter { get{ return isLetter;  } set{ isLetter = value; } }

		[SerializeField] private int orderIndex;
		public int OrderIndex { get{ return orderIndex; } set{ orderIndex = value; } }
		
		
		[SerializeField] private string uid;
		public string Uid { get{ return uid; } set{ uid = value; } }
		
		[SerializeField] private TextMeshPro numberText;
		public TextMeshPro NumberText { get{ return numberText; } set{ numberText = value; } }
		
		
		
		[SerializeField] private bool isCorrect;
		public bool IsCorrect { get{ return isCorrect; } set{ isCorrect = value; } }
		
		[SerializeField] private SpriteRenderer starSpriteRenderer;
		public SpriteRenderer StarSpriteRenderer { get{ return starSpriteRenderer; } set{ starSpriteRenderer = value; } }
		[SerializeField] private Color[] starColours;
		[SerializeField] private List<Collider2D> proximityStars;
		[SerializeField] public List<Collider2D> ProximityStars { get{ return proximityStars; } set{ proximityStars = value; } }

		[SerializeField] private TextMeshPro starText;

		public string StarValue { get { return starText.text; } }

		[SerializeField] private float starPosX, starPosY;
		public float StarPositionX { get{ return starPosX; } set{ starPosX = value; } }
		public float StarPositionY { get{ return starPosY; } set{ starPosY = value; } }
		[SerializeField] private int colourCode = 1;
		public int Colourcode { get{ return colourCode; } set{ colourCode = value; } }

		[SerializeField] private Color originalColour;

		public Color OriginalColour { get{ return originalColour; } set{ originalColour = value; } }

		[SerializeField] private string starName;
		public string StarName { get{ return starName; } set{ starName = value; } }

		[SerializeField] private string distractor;
		public string Distractor { get{ return distractor; } set{ distractor = value; } }

		[SerializeField] private string targetDistractorID;
		public string TargetResponseID { get{ return targetDistractorID; }  set{ value = targetDistractorID; } }

    	private void Start()
		{
			originalColour = starSpriteRenderer.color;
			_transform = transform;	
		}

		private void OnEnable()
		{
			starName = gameObject.name;
			Messenger.AddListener( "Disable" , Disable );
			Messenger<int>.AddListener( "NextStar", NextStar );
			Messenger.AddListener( "ProximityCheck" , FindProximityStars );
			
			_transform = transform;
			uid = CreateUID();
			
			//starSpriteRenderer.color = starColours[ Random.Range( 0, starColours.Length -1 ) ];

			starText = transform.Find( "StarText" ).GetComponent<TextMeshPro>();

			starPosX = transform.position.x;
			starPosY = transform.position.y;

			if(SessionManager.Instance && !isRandom )
				SessionManager.Instance.SetStarInfo( gameObject.GetComponent<Star>() );
			//SessionManager.Instance.SetStarInfo( starPosX, starPosY, colourCode );
			//GetComponent<Collider2D>().enabled = true;
		}

		private void OnDisable()
		{
			GetComponent<Collider2D>().enabled = true;
			Messenger.RemoveListener( "Disable" , Disable );
			Messenger<int>.RemoveListener( "NextStar", NextStar  );
			Messenger.RemoveListener( "ProximityCheck" , FindProximityStars );
		}

		private string CreateUID()
		{
			return System.Guid.NewGuid().ToString();
		}

		[SerializeField] private int touchCount = 0;

		private void OnMouseDown()
		{	
			SessionManager.Instance.StopTimer();

			SessionManager.Instance.CreateSelection();

			SessionManager.Instance.CalculateRT();
			
			SessionManager.Instance.StartTimer();
		
			SessionManager.Instance.SetTargetStar();

			SessionManager.Instance.CurrentStar = gameObject.GetComponent<Star>();

			if( starText )
			{
				SessionManager.Instance.SetResponse( starText.text );
				//SessionManager.Instance.SetTargetDistractor( distractor );	
			}
			
			
			SessionManager.Instance.SetResponseLocation( starPosX, starPosY );

			SessionManager.Instance.SetResponseGameObject( this.gameObject );

			SessionManager.Instance.DistanceFromTarget();

			if( isCorrect )
			{
				CorrectSelection();
			}
			else
			{
				IncorrectSelection();
			}

			SessionManager.Instance.EndSelection();

			//StarManager.Instance.LastStarSelected = this.gameObject;
		}

		private void CorrectSelection()
		{
			//SessionManager.Instance.SetTargetResponseLocation( starPosX, starPosY );
			
			//SessionManager.Instance.SetTargetDistractor( distractor );
			
			SessionManager.Instance.LevelLayoutCount ++;

			SessionManager.Instance.SetCorrect( 1 );
				
			GetComponent<StarFxHandler>().PunchScale( this.gameObject );
			GetComponent<StarFxHandler>().ColourChange( starSpriteRenderer , new Color( 0.9716f, 0.8722f, 0.1512f, 1 ) );
			isCorrect = false;

			Messenger<int>.Broadcast( "NextStar", orderIndex + 1 );

			if( isTutorial )
				GameManager.Instance.Score ++;
			
			if( !isTutorial )
			{
				Messenger.Broadcast( "UpdateScore" );
				//Messenger.Broadcast( "AddTime" );
			}
			//Broadcast message to all stars
			//pass the next starcount
			
			SessionManager.Instance.PreviousStar = gameObject.GetComponent<Star>();

			GetComponent<Collider2D>().enabled = false;		
		}

		private void IncorrectSelection()
		{
			//SessionManager.Instance.SetTargetStar();
		
			SessionManager.Instance.SetCorrect( 0 );

			if( IsLetter )
				SessionManager.Instance.CheckForPreservativeError();
			
			SessionManager.Instance.CheckForProximityError();
			
			GetComponent<StarFxHandler>().Shake( this.gameObject );
			GetComponent<StarFxHandler>().ColourFade( starSpriteRenderer, starSpriteRenderer.color );
			StartCoroutine( TempDisableButton() );
			
			// if( !isTutorial )
			// 	Messenger.Broadcast( "SubtractTime" );
		}


		private void Disable()
		{	
			gameObject.SetActive( false );
			StarPool.Instance.ReturnToPool( this );
		}

		private void NextStar( int nextStar )
		{
			if( nextStar == orderIndex )
				isCorrect = true;
		}
		
		private void FindProximityStars()
		{
			int radius = 10;
			
			proximityStars.Clear();
			proximityStars = ProximityHandler.Instance.FindProximityStars( gameObject.transform, radius );	
		}

		private IEnumerator TempDisableButton()
		{
			GetComponent<Collider2D>().enabled = false;	
			yield return new WaitForSeconds( 1.25f );
			GetComponent<Collider2D>().enabled = true;	
		}

		public void TriggerCorrectSelection()
		{
			
			GetComponent<StarFxHandler>().PunchScale( this.gameObject );
			GetComponent<StarFxHandler>().ColourChange( starSpriteRenderer , new Color( 0.9716f, 0.8722f, 0.1512f, 1 ) );
		}

		public void TriggerInCorrectSelection()
		{		
			GetComponent<StarFxHandler>().Shake( this.gameObject );
			GetComponent<StarFxHandler>().ColourFade( starSpriteRenderer, starSpriteRenderer.color );
		}

		public void DisableCollider( )
		{
			gameObject.transform.GetComponent<Collider2D>().enabled = false;
		}

		public void EnableCollider()
		{
			gameObject.transform.GetComponent<Collider2D>().enabled = true;
		}

		public void ResetColour()
		{
			starSpriteRenderer.color = originalColour;
		}

	}
}
