using UnityEngine;


public class ScreenShot : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	

	//At the moment this takes a simple screen shot 
	//Need to add user info to the screenshot.
	public void TakeScreenShot()
	{
		Debug.Log( System.DateTime.Now );
		ScreenCapture.CaptureScreenshot( "Trail_Making_Test_UserName_" + System.DateTime.Now.Second+".png" );
	}
}
