using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Star : MonoBehaviour 
{
	private Transform _transform;

	[SerializeField] private int orderIndex;
	public int OrderIndex { get{ return orderIndex; } set{ orderIndex = value; } }
	
	[SerializeField] private float proximityRadius;
	[SerializeField] private string uid;
	
	[SerializeField] private TextMeshPro numberText;
	public TextMeshPro NumberText { get{ return numberText; } set{ numberText = value; } }
	
	[SerializeField] private float vertExtent;
	[SerializeField] private float horzExtent;
	[SerializeField] private Collider2D[] hits;
	[SerializeField] private bool isValidPosition;
	
	[SerializeField] private bool isCorrect;
	public bool IsCorrect { get{ return isCorrect; } set{ isCorrect = value; } }
	
	[SerializeField] private SpriteRenderer starSpriteRenderer;
	[SerializeField] private Color[] starColours;
	[SerializeField] private List<Collider2D> proximityStars;

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

		starSpriteRenderer.color = starColours[ Random.Range( 0, starColours.Length -1 ) ];
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

	private void OnMouseDown()
	{
		if( isCorrect )

			CorrectSelection();
		else
			IncorrectSelection();
	}

	private void CorrectSelection()
	{
		StarFxHandler.Instance.PunchScale( this.gameObject );
		StarFxHandler.Instance.ColourChange( starSpriteRenderer , new Color( 0.9716f, 0.8722f, 0.1512f, 1 ) );
		isCorrect = false;

		Messenger.Broadcast( "UpdateScore" );
		//Broadcast message to all stars
		//pass the next starcount
		Messenger<int>.Broadcast( "NextStar", orderIndex + 1 );

		GetComponent<Collider2D>().enabled = false;
	}

	private void IncorrectSelection()
	{
		StarFxHandler.Instance.Shake( this.gameObject );
		StarFxHandler.Instance.ColourFade( starSpriteRenderer, starSpriteRenderer.color );
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
}
