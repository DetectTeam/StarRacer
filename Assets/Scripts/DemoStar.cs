using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoStar : MonoBehaviour 
{

	[SerializeField] private SpriteRenderer starSpriteRenderer;
	[SerializeField] private Color originalColour; //Original Colour of Star ( Purple ) used as a buffer

	void Start()
	{
		originalColour = starSpriteRenderer.color;
		
	}

	void OnTriggerEnter2D( Collider2D other )
	{
			//if( other.name.Equals( "Line3D" ) || other.name.Equals( "Line2D" ) )
			if( other.name.Equals( "DemoPointer" ) )
			{
				StartCoroutine( "StarSequence" );
			}
	}	

	private IEnumerator StarSequence()
	{
		yield return null;

		Messenger<int>.Broadcast( "PlaySound" , 0 );
		iTween.PunchScale( starSpriteRenderer.gameObject, iTween.Hash( "x",-2, "y",-2, "time",0.75f));
		starSpriteRenderer.color = new Color( 0.9716f, 0.8722f, 0.1512f, 1 );
		originalColour = starSpriteRenderer.color;
		//GameManager.Instance.UpdateScore();
	}
	
}
