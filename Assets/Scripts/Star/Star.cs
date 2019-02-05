using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace StarRacer
{
	public class Star : MonoBehaviour 
	{
		private Transform _transform;

		public bool IsLetter { get; set; }

		[SerializeField] private int orderIndex;
		public int OrderIndex { get{ return orderIndex; } set{ orderIndex = value; } }
		
		[SerializeField] private float proximityRadius;
		[SerializeField] private string uid;
		public string Uid { get{ return uid; } set{ uid = value; } }
		
		[SerializeField] private TextMeshPro numberText;
		public TextMeshPro NumberText { get{ return numberText; } set{ numberText = value; } }
		
		[SerializeField] private float vertExtent;
		[SerializeField] private float horzExtent;
		[SerializeField] private Collider2D[] hits;
		[SerializeField] private bool isValidPosition;
		
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

		private string starName;
		public string StarName { get{ return starName; } set{ starName = value; } }

		[SerializeField] private string distractor;

    	private void Start()
		{
			SetCameraBounds();
			_transform = transform;	
		}

		private void OnEnable()
		{
			Messenger.AddListener( "Disable" , Disable );
			Messenger<int>.AddListener( "NextStar", NextStar );
			Messenger.AddListener( "ProximityCheck" , FindProximityStars );
			
			SetCameraBounds();
			_transform = transform;
			uid = CreateUID();
			FindPositionToSpawn();

			//starSpriteRenderer.color = starColours[ Random.Range( 0, starColours.Length -1 ) ];

			starText = transform.Find( "StarText" ).GetComponent<TextMeshPro>();

			starPosX = transform.position.x;
			starPosY = transform.position.y;

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

		private void FindPositionToSpawn()
		{
			isValidPosition = false;

			while( !isValidPosition )
			{
				_transform.position = GetRandomPosition();

				hits = Physics2D.OverlapCircleAll( transform.position, proximityRadius );

				if( hits.Length <= 1 )
				{
					isValidPosition = true;
				}
			}	
		}

		private Vector3 GetRandomPosition()
		{
			return new Vector3( Random.Range( -horzExtent, horzExtent ), Random.Range( -vertExtent, vertExtent - 3 ), _transform.position.z  ) ;
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
				SessionManager.Instance.SetResponse( starText.text );

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

			StarManager.Instance.LastStarSelected = this.gameObject;

	
		}

		private void CorrectSelection()
		{
			//SessionManager.Instance.SetTargetResponseLocation( starPosX, starPosY );
			
			SessionManager.Instance.LevelLayoutCount ++;

			SessionManager.Instance.SetCorrect( 1 );
				
			GetComponent<StarFxHandler>().PunchScale( this.gameObject );
			GetComponent<StarFxHandler>().ColourChange( starSpriteRenderer , new Color( 0.9716f, 0.8722f, 0.1512f, 1 ) );
			isCorrect = false;

			Messenger.Broadcast( "UpdateScore" );
			Messenger.Broadcast( "AddTime" );
			//Broadcast message to all stars
			//pass the next starcount
			Messenger<int>.Broadcast( "NextStar", orderIndex + 1 );


			SessionManager.Instance.PreviousStar = gameObject.GetComponent<Star>();

			GetComponent<Collider2D>().enabled = false;	
			
		}

		private void IncorrectSelection()
		{
			SessionManager.Instance.SetTargetStar();

			SessionManager.Instance.SetCorrect( 0 );

			SessionManager.Instance.CheckForProximityError();
			
			if( IsLetter )
				SessionManager.Instance.CheckForPreservativeError();
			
			GetComponent<StarFxHandler>().Shake( this.gameObject );
			GetComponent<StarFxHandler>().ColourFade( starSpriteRenderer, starSpriteRenderer.color );
			StartCoroutine( TempDisableButton() );
			Messenger.Broadcast( "SubtractTime" );
		}

		private void SetCameraBounds()
		{
			vertExtent = Camera.main.orthographicSize - 2;    
			horzExtent = ( vertExtent * Screen.width / Screen.height );
			horzExtent = horzExtent - 1;
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

	}
}
