using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Spawn gameobjects at a random position and minimum distance from eachother within the confines of the camera view
namespace StarRacer
{
	public class RandomSpawner : MonoBehaviour 
	{

		[SerializeField] private float vertExtent;
		[SerializeField] private float horzExtent;
		[SerializeField] private float proximityRadius;
		[SerializeField] private bool isValidPosition;
		[SerializeField] private Collider2D[] hits;

		private Transform cachedTransform;

		private void Awake()
		{
			cachedTransform = transform;
		}

		private void OnEnable()
		{
			SetCameraBounds();
			FindPositionToSpawn();
		}

		private void FindPositionToSpawn()
		{
			isValidPosition = false;

			while( !isValidPosition )
			{
				transform.position = GetRandomPosition();

				hits = Physics2D.OverlapCircleAll( transform.position, proximityRadius );

				if( hits.Length <= 1 )
				{
						isValidPosition = true;
				}
			}
			//Debug.Log( "SEtting new star position...." );
			
			Star star = gameObject.GetComponent<Star>();

			star.StarPositionX = transform.position.x;
			star.StarPositionY = transform.position.y;

			SessionManager.Instance.SetStarInfo( star );
		}

		private Vector3 GetRandomPosition()
		{
			return new Vector3( Random.Range( -horzExtent, horzExtent ), Random.Range( -vertExtent, vertExtent - 3 ), cachedTransform.position.z  ) ;
		}

		private void SetCameraBounds()
		{
			vertExtent = Camera.main.orthographicSize - 2;    
			horzExtent = ( vertExtent * Screen.width / Screen.height );
			horzExtent = horzExtent - 1;
		}
		
	}
}
