using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProximityHandler : MonoBehaviour  
{
	public static ProximityHandler Instance;
	private void Awake()
	{
		if( Instance == null )
		{
			Instance = this;
		}
		else if( Instance != this )
		{
			Destroy( gameObject );
		}
	}

	public  List<Collider2D> FindProximityStars( Transform objTransform, float radius )
	{
		Collider2D[] hits = null;
		
		hits = Physics2D.OverlapCircleAll( objTransform.position, radius, 1 << LayerMask.NameToLayer( "Star" ) );

		return hits.OrderBy( x => Vector2.Distance( objTransform.position, x.transform.position ) ).ToList();			
	}
}
