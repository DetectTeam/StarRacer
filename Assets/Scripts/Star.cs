using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSessions;
using TMPro;

public class Star : MonoBehaviour 
{

	[SerializeField] private int starValue = 0;
	[SerializeField] private bool isStarLetter;

	[SerializeField] private GameObject[] proximityStars = new GameObject[ 5 ];

	public GameObject[] ProximityStars { get{ return proximityStars; } }


	public bool IsStarLetter { get{ return isStarLetter; } set{ isStarLetter = value; } }


	[SerializeField] private bool isFirstStar;
	

//	private LinkHandler linkHandler;
	private Transform _transform;
	private Material _material;
	private Color originalColour; //Original Colour of Star ( Purple ) used as a buffer
	private Color errorColour = new Color( 0.9433f, 0.1382f, 0.0934f ); //Red
	private Color successColour; //Gold 
	private DrawLineMouse _drawLineHandler;
	//private bool isOkToDrawLine;
	//private bool isOkToColor = false;
	private GameManager gameManager;
	private SessionManager sessionManager;
	private TransitionManager transitionManager;
	private SpriteRenderer starSpriteRenderer;
	private Transform star;

	private TextMeshPro starText;

	[SerializeField] private Timer _timer;

	void Start()
	{
		_transform = transform;

		

		//Find Sibling Star . The star in question has a sprite renderer that needs to be accessed
		//Bad Eoghan ..you shouldnt use Find ...REfactor later
		star = _transform.parent.Find( "Star" );

		_timer =  GameObject.Find("Timer").GetComponent<Timer>();

		starText = _transform.parent.GetComponent<TextMeshPro>(); //Grab the text value from parent


		ParallaxMovement ParallaxMovement = star.GetComponent<ParallaxMovement>();
		ParallaxMovement.enabled = false;

		if( star != null )
			starSpriteRenderer = star.GetComponent<SpriteRenderer>();
		
		//linkHandler = Object.FindObjectOfType<LinkHandler>();
		gameManager = Object.FindObjectOfType<GameManager>();

		if( gameManager != null )
		{
			sessionManager = gameManager.GetComponent<SessionManager>();
			transitionManager = gameManager.GetComponent<TransitionManager>();
		}


		_material = GetComponent<Renderer>().material;

		_drawLineHandler = GameObject.Find( "Main Camera" ).GetComponent<DrawLineMouse>();

		originalColour = starSpriteRenderer.color;
		
		//isOkToDrawLine = true; //A check to prevent the player from drawing a line to the wrong selection twice

		//Make sure that lastnode is always set
		//Especially at the start of a level
		if( gameObject.name.Equals( "Star 1" ) )
		{
			_drawLineHandler.LastNode = transform;
		}
	}

	//Detect when line intersects star 
	float tmpTime;
	void OnTriggerEnter2D( Collider2D other )
	{	
	
		

	   if( other.name.Equals( "DrawnLine" ) || other.name.Equals( "DemoShip" ) )
	   {
		   	Debug.Log( "ENTERED TRIGGER" );

			if( _timer && starValue == 0 )
			{
				sessionManager.TimeToStartSession = _timer.TimeTakenSeconds.ToString();
			}

			StartCoroutine( "StarSequence" );

	   }

	}

	void OnTriggerExit2D( Collider2D other )
	{
			
	}

	IEnumerator StarSequence()
	{
		yield return new WaitForSeconds( 0.1f );

		//Check if last star clicked is one less than current star
		if( StarManager.previousStar == ( starValue - 1 ) )
		{
			//Check if Transition manager exists and check if a transition is already active
			if( transitionManager != null &&  !transitionManager.IsTransitionOpen )
			{
				//If a transition is not active then Create one
				transitionManager.CreateTransition();

				//Set the source for the transition 
				transitionManager.TransitionSource = gameObject.name;
			}
			else
			{
				//Set the transitions destination ..only if its the correct destination
				transitionManager.TransitionDestination = gameObject.name;
				
				//End the transition and add it to the transition list
				Transition currentTransition = transitionManager.EndTransition(); 

				if( currentTransition != null )
				{
					sessionManager.AddTransition( currentTransition );	
				}
				
				//Start a new Transition.
				transitionManager.CreateTransition();
				//Set the source for the transition
				transitionManager.TransitionSource = gameObject.name;
			}
			
			//Set the star materials color to gold ( indicating success )
			//_material.color = successColour;

			if( GameManager.Instance.IsDemoMode )
			{
				Messenger<int>.Broadcast( "PlaySound" , 0 );
				iTween.PunchScale( starSpriteRenderer.gameObject, iTween.Hash( "x",-2, "y",-2, "time",0.75f));
				starSpriteRenderer.color = new Color( 0.9716f, 0.8722f, 0.1512f, 1 );
				originalColour = starSpriteRenderer.color;
				gameManager.UpdateScore();
			}
			else if( _drawLineHandler )
			{
				StarManager.lastSuccessPointCount = _drawLineHandler.PointCount;
				_drawLineHandler.DrawLine( _drawLineHandler.PointCount );
				_drawLineHandler.LastNode = _transform;
				
				//Refactor replace with message
				gameManager.UpdateScore();


				//Play Success sound effect
				//Message broadcast : PlaySound takes an int as a parameter 
				//Subscribers : SoundManager
				//Messenger<int>.Broadcast( "RandomizePitch", 0 );
				Messenger<int>.Broadcast( "PlaySound" , 0 );
				//Handheld.Vibrate();

				//Update the progress star with the current star
				//Progress star will display the current
				Messenger<string>.Broadcast( "UpdateProgress" , starText.text );

				//Punch animation when correct star is encountered
				iTween.PunchScale( starSpriteRenderer.gameObject, iTween.Hash( "x",-2, "y",-2, "time",0.75f));

				starSpriteRenderer.color = new Color( 0.9716f, 0.8722f, 0.1512f, 1 );

				originalColour = starSpriteRenderer.color;
				
				if( _drawLineHandler.IsMouseUp )
				{
					_drawLineHandler.CanDraw = false;
				}
	

			}

			StarManager.previousStar = starValue;
			StarManager.previousStarObject = this;

		}
		else if( StarManager.previousStar == starValue )
		{
			//Do Absolutely nothing for the moment...
		}
		else //Error E.G. Player clicked on wrong star
		{
		    
			Star previousStarObject = StarManager.previousStarObject;

			transitionManager.TransitionErrorCount ++; //Total number or errors for this transition

			// Error err = new Error(); //Create new instance of error class

			// err.Source = previousStarObject.name;
			// err.Destination = this.name;
			// err.ErrorTimeStamp = System.DateTime.Now.ToString();

			//Check previous stars list of proximity stars
			//if a player accidently hits a star contained in this list , then that constitutes a proximity error
			if( previousStarObject.ProximityStars[0] != null )
			{
			
				foreach( GameObject g in previousStarObject.ProximityStars )
				{
					if( this.name.Equals( g.name ) && g.name != null )
					{
						//err.ProximityError = true;
						transitionManager.ProximityErrorCount ++;

					}
				}

			}


			//Check the type of error that occured
			if( previousStarObject.IsStarLetter && this.IsStarLetter ) //letter to letter error
			{
				//err.PreservativeError = true;
				transitionManager.PreservativeErrorCount ++;
			}
			else if( !previousStarObject.IsStarLetter && !this.IsStarLetter ) //number to number error
			{
				//err.PreservativeError = true;
				transitionManager.PreservativeErrorCount ++;
			}
			else if( !previousStarObject.IsStarLetter && this.IsStarLetter ) //number to letter error
			{
				//err.NumberToLetterError = true;
				transitionManager.NumToLetterErrorCount ++;
			}
			else if( previousStarObject.IsStarLetter && !this.IsStarLetter ) //letter to number error
			{
				transitionManager.LetterToNumErrorCount ++;
				//err.LetterToNumberError = true;
			}

			//transitionManager.AddError( err ); //Add the error to the error list for this transition
		
			//Consequences of picking the wrong star . Make star shake 
			iTween.ShakePosition( _transform.parent.gameObject, new Vector2( 0.2f, 0.2f ), 0.75f );
			//Handheld.Vibrate();
		
			//Play Error Sound
			//Message broadcast : PlaySound takes an int as a parameter 
			//Subscribers : SoundManager
			Messenger<int>.Broadcast( "PlaySound" , 1 );

			//Tween between stars original colour and red and back
			//isOkToColor = true;
			    iTween.ValueTo (gameObject, iTween.Hash (
					"from", errorColour, 
					"to", originalColour, 
					"time", 1.25f, 
					"easetype", "easeInCubic", 
					"onUpdate","UpdateColor"));
			
			_drawLineHandler.DestroyLine();
			_drawLineHandler.CreateLine();

		
		}
	
	}

	void UpdateColor(Color newColor)
 	{
    	 starSpriteRenderer.color = newColor;
 	}

	
	//Dont let the player start a line in open space.
	//Force them to start drawing form the last success node.
	void OnMouseDown()
	{
		
		if( _drawLineHandler.LastNode.name.Equals( gameObject.name ) )
		{
			_drawLineHandler.CanDraw = true;
		}
	}

}
