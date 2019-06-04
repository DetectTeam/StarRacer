using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionHandler : MonoBehaviour 
{


	[SerializeField] private bool isValidPosition;
	[SerializeField] private bool isFirst;

	[SerializeField] private float vertExtent;
	[SerializeField] private float horzExtent;

	[SerializeField] private float proximityRadius;


	// public static ProximityHandler Instance;

	// private void Awake()
	// {
	// 	if( Instance == null )
	// 	{
	// 		Instance = this;
	// 	}
	// 	else if( Instance != this )
	// 	{
	// 		Destroy( gameObject );
	// 	}
	// }

	private void OnEnable()
	{
		SetCameraBounds();
		//CalculatespawnPosition();
	}

	// private void Start()
	// {
	// 	_transform = transform;
	// 	SetCameraBounds();
	// }

	

	public void CalculatespawnPosition( )
	{
		Debug.Log( "Calc Spawn Position...." );

		
		
		while( !isValidPosition )
		{
			transform.position = GetRandomPosition();

			Debug.Log( transform.position );

			var hits = Physics2D.OverlapCircleAll( transform.position, proximityRadius );

			if( hits.Length <= 1  )
			{
				isValidPosition = true;
			}
		}
	}

	private Vector3 GetRandomPosition()
	{
		Debug.Log( "Getting Random Position..." );
		return new Vector3( Random.Range( -horzExtent, horzExtent ), Random.Range( -vertExtent, vertExtent - 3 ), transform.position.z  ) ;
	}

	private void SetCameraBounds()
	{
		vertExtent = Camera.main.orthographicSize - 2;    
        horzExtent = ( vertExtent * Screen.width / Screen.height );
		horzExtent = horzExtent - 1;
	}
	
}
