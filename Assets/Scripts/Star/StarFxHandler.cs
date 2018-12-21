using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFxHandler : MonoBehaviour 
{



	[SerializeField] private Color errorColour;
	[SerializeField] private SpriteRenderer spriteRenderer;
	public void ColourFade( SpriteRenderer sRenderer , Color color )
	{
		 spriteRenderer = sRenderer;

		 iTween.ValueTo (gameObject, iTween.Hash (
					"from", errorColour, 
					"to", color, 
					"time", 1.25f, 
					"easetype", "easeInCubic", 
					"onUpdate","UpdateColor"));
	}

	private void UpdateColor(Color newColor)
 	{
    	 spriteRenderer.color = newColor;
 	}

	public void ColourChange( SpriteRenderer sRenderer , Color color )
	{
		sRenderer.color = color;
	} 


	public void PunchScale( GameObject obj )
	{
		iTween.PunchScale( obj, iTween.Hash( "x", +5, "y", +5, "time", 1f ) );
	}



	public void Shake( GameObject obj )
	{
		iTween.ShakePosition( obj, new Vector2( 0.2f, 0.2f ), 0.75f );
	} 
}
